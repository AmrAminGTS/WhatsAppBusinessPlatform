using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Application.Common;

namespace WhatsAppBusinessPlatform.Application.Abstractions.Shared;

public interface IIdempotencyHandler
{
    Task<Result<TResponse>> HandleWithIdempotencyAsync<TResponse>(
        string idempotencyKey,
        Func<Task<Result<TResponse>>> criticalFunc,
        int timeToLiveInMinutes = 5,
        int lockAcquiringTimeout = 30,
        CancellationToken cancellationToken = default);
}

