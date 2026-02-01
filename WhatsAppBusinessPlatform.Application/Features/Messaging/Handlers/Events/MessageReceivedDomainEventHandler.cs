using System.Text.Json;
using LinqKit;
using MediatAmR.Abstractions;
using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Abstractions.Realtime;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Events;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Chats;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

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

internal sealed class MessageReactionsChangedDomainEventHandler(IServerEventChannel ServerEventChannel, IUnitOfWork unitOfWork)
    : IDomainEventHandler<MessageReactionsChangedDomainEvent>
{
    public async Task HandleAsync(MessageReactionsChangedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        ChatLastUpdateResponse? lastChatUpdate = @event.Emoji != null
        ? new()
        {
            Direction = MessageDirection.Received,
            Message = null,
            Reaction = new MessageReactionResponse(@event.Emoji, @event.ReactedToMessageContent),
            IsReaction = true,
            When = @event.When
        }
        : await unitOfWork.AccountRepository
            .Where(a => a.PhoneNumber == @event.ContactPhone)
            .AsExpandable()
            .Select(a => WAAccount.GetLastUpdateExpr().Invoke(a))
            .SingleOrDefaultAsync(cancellationToken);

        var payload = new { @event.ContactPhone, @event.MessageId, @event.ReactionSummary, lastChatUpdate };
        await ServerEventChannel.BroadcastEventAsync(new(payload, "message-reaction-updated"), cancellationToken);
    }
}

