using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.ServerSentEvents;
using System.Text;
using System.Threading.Channels;

namespace WhatsAppBusinessPlatform.Application.Abstractions.Realtime;

public interface IServerEventChannel : IDisposable
{
    ConcurrentDictionary<Guid, Channel<SseItem<object>>> Subscribers { get; }

    ChannelReader<SseItem<object>> Subscribe();
    void Unsubscribe(Guid id);
    Task WriteEventAsync(Guid userId, SseItem<object> sseItem, CancellationToken cancellationToken = default);
    Task BroadcastEventAsync(SseItem<object> sseItem, CancellationToken cancellationToken = default);
}
