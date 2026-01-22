using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Contacts;

namespace WhatsAppBusinessPlatform.Infrastucture.Shared;

internal sealed class IdempotencyHandler : IIdempotencyHandler, IDisposable
{
    private readonly ILogger<IdempotencyHandler> _logger;
    private readonly IMemoryCache _memoryCache;
    // keyed locks to prevent High Contention problem
    private readonly ConcurrentDictionary<string, LockWrapper> _locks = new();
    private bool _disposed;

    public IdempotencyHandler(
        ILogger<IdempotencyHandler> logger,
        IMemoryCache memoryCache)
    {
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public async Task<Result<TResponse>> HandleWithIdempotencyAsync<TResponse>(
        string idempotencyKey,
        Func<Task<Result<TResponse>>> criticalFunc,
        int timeToLiveInMinutes = 5,
        int lockAcquiringTimeout = 30,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(idempotencyKey);
        ArgumentNullException.ThrowIfNull(criticalFunc);

        if (_memoryCache.TryGetValue(idempotencyKey, out Result<TResponse>? cached))
        {
            _logger.LogDebug("Idempotency cache hit for key {Key}", idempotencyKey);
            return cached!;
        }

        // Acquire (or create) wrapper and bump ref-count
        LockWrapper wrapper = _locks.GetOrAdd(idempotencyKey, _ => new LockWrapper());
        Interlocked.Increment(ref wrapper.RefCount);

        bool acquired = false;
        try
        {
            acquired = await wrapper.Semaphore.WaitAsync(TimeSpan.FromSeconds(lockAcquiringTimeout), cancellationToken);
            if (acquired == false)
            {
                throw new TimeoutException("Couldn't acquire the lock (the process inside is taking too long), Try again later");
            }

            // Check cache after acquiring lock (prevents TOCTOU)
            if (_memoryCache.TryGetValue(idempotencyKey, out cached))
            {
                _logger.LogDebug("Idempotency cache hit for key {Key}", idempotencyKey);
                return cached!;
            }

            // Execute 
            Result<TResponse> result = await criticalFunc();

            // Set Cache on success
            if (result.IsSuccess)
            {
                if (!_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogInformation("Debug level is currently DISABLED for this logger.");
                }

                _memoryCache.Set(idempotencyKey, result, TimeSpan.FromMinutes(timeToLiveInMinutes));
                _logger.LogDebug("Cached idempotent result for key {Key} for {Minutes} minutes", idempotencyKey, timeToLiveInMinutes);
            }
            return result;
        }
        finally
        {
            if (acquired)
            {
                wrapper.Semaphore.Release();
            }

            // decrement ref-count and remove+dispose if zero
            if (Interlocked.Decrement(ref wrapper.RefCount) == 0 
                && _locks.TryRemove(new KeyValuePair<string, LockWrapper>(idempotencyKey, wrapper)))
            {
                DisposeSemaphore(idempotencyKey, wrapper);
            }
        }
    }
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;

        foreach (KeyValuePair<string, LockWrapper> kvp in _locks)
        {
            DisposeSemaphore(kvp.Key, kvp.Value);
        }
    }

    // Private Members
    private sealed class LockWrapper
    {
        public SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(1, 1);
        // refCount is mutated with Interlocked
        public int RefCount;
    }
    private void DisposeSemaphore(string idempotencyKey, LockWrapper wrapper)
    {
        try
        {
            wrapper.Semaphore.Dispose();
        }
        catch (ObjectDisposedException objctDisposedEx)
        {
            _logger.LogDebug(objctDisposedEx, "Semaphore already disposed for key {Key}", idempotencyKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Unexpected error disposing semaphore for key {Key}", idempotencyKey);
        }
    }
}
