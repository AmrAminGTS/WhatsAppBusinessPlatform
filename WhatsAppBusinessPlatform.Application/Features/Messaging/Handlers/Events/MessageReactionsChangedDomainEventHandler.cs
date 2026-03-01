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

internal sealed class MessageReactionsChangedDomainEventHandler(IRealTimeMessagingChannel realTimeMessagingChannel, IUnitOfWork unitOfWork)
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

        MessageReactionsChangedResponse payload = new()
        {
            ContactPhone = @event.ContactPhone,
            MessageId = @event.MessageId,
            ReactionSummary = @event.ReactionSummary,
            LastChatUpdate = lastChatUpdate
        };
        await realTimeMessagingChannel.MessageReactionsChangedAsync(payload, cancellationToken);
    }
}

