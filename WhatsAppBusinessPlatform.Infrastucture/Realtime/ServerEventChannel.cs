using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.ServerSentEvents;
using System.Text;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using WhatsAppBusinessPlatform.Application.Abstractions.Realtime;

namespace WhatsAppBusinessPlatform.Infrastucture.Realtime;

public class ServerEventChannel(ILogger<ServerEventChannel> logger) : IServerEventChannel
{
    // each subscriber gets its own channel writer/reader
    public ConcurrentDictionary<Guid, Channel<SseItem<object>>> Subscribers { get; } = new();

    private bool _disposed;
    public ChannelReader<SseItem<object>> Subscribe()
    {
        var id = Guid.NewGuid();
        var channel = Channel.CreateUnbounded<SseItem<object>>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

        Subscribers[id] = channel;
        return channel.Reader;
    }
    public void Unsubscribe(Guid id)
    {
        if (Subscribers.TryRemove(id, out Channel<SseItem<object>>? ch))
        {
            ch.Writer.TryComplete();
        }
    }

    // broadcast to all connected users
    public Task BroadcastEventAsync(SseItem<object> sseItem, CancellationToken cancellationToken = default)
    {
        foreach (KeyValuePair<Guid, Channel<SseItem<object>>> kv in Subscribers)
        {
            WriteEventAsync(kv.Key, sseItem, cancellationToken);
        }

        return Task.CompletedTask;
    }
    public Task WriteEventAsync(Guid userId, SseItem<object> sseItem, CancellationToken cancellationToken = default)
    {
        bool isSubscribed = Subscribers.TryGetValue(userId, out Channel<SseItem<object>>? ch);
        if (!isSubscribed)
        {
            logger.LogWarning("The user: {Key} __ isn't subscribed", userId);
        }

        // best-effort: try write, don't block publish for slow clients
        ChannelWriter<SseItem<object>> writer = ch!.Writer;
        _ = writer.WaitToWriteAsync(cancellationToken).AsTask().ContinueWith(t =>
        {
            if (!t.IsCompletedSuccessfully || !writer.TryWrite(sseItem))
            {
                logger.LogError("Couldn't write event to channel: {Key}", userId);
            }
        }, TaskScheduler.Default);
        return Task.CompletedTask;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Private Methods
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            foreach (KeyValuePair<Guid, Channel<SseItem<object>>> kv in Subscribers)
            {
                kv.Value.Writer.TryComplete();
            }
            Subscribers.Clear();
        }

        _disposed = true;
    }
}
