using MediatAmR.Abstractions;
using WhatsAppBusinessPlatform.Application.Abstractions.Realtime;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Events;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Chats;

namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Handlers.Events;
internal sealed class MessageReceivedDomainEventHandler(IServerEventChannel ServerEventChannel) 
    : IDomainEventHandler<MessageReceivedDomainEvent>
{
    public async Task HandleAsync(MessageReceivedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        RealTimeChatMessageResponse payload = @event.Message.MapToRealTimeChatMessageResponse();
        await ServerEventChannel.BroadcastEventAsync(new(payload, "message-received"), cancellationToken);
    }
}

internal sealed class MessageReactionsChangedDomainEventHandler(IServerEventChannel ServerEventChannel)
    : IDomainEventHandler<MessageReactionsChangedDomainEvent>
{
    public async Task HandleAsync(MessageReactionsChangedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        var messageReactions = @event.ReactedToMessage.Reactions
            .GroupBy(r => r.Emoji)
            .Select(g => new
            {
                Emoji = g.Key,
                Count = g.Count(),
            })
            .ToList();
        await ServerEventChannel.BroadcastEventAsync(new(messageReactions, "message-Reactions-Changed"), cancellationToken);
    }
}
