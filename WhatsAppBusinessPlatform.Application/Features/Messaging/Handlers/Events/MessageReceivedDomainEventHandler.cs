using System.Text.Json;
using MediatAmR.Abstractions;
using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Abstractions.Realtime;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Events;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Chats;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Handlers.Events;
internal sealed class MessageReceivedDomainEventHandler(IRealTimeMessagingChannel realTimeMessagingChannel, IUnitOfWork unitOfWork) 
    : IDomainEventHandler<MessageReceivedDomainEvent>
{
    public async Task HandleAsync(MessageReceivedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        WAMessage message = await unitOfWork.Repository<WAMessage>()
            .Include(m => m.ReplyTo)
            .Where(m => m.Id == @event.MessageId)
            .SingleAsync(cancellationToken);

        RealTimeChatMessageResponse payload = message.MapToRealTimeChatMessageResponse();
        await realTimeMessagingChannel.MessageReceivedAsync(payload, cancellationToken);
    }
}
