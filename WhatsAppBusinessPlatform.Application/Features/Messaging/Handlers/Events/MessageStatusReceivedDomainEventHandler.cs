using System;
using System.Collections.Generic;
using System.Net.ServerSentEvents;
using System.Text;
using System.Threading.Channels;
using MediatAmR.Abstractions;
using WhatsAppBusinessPlatform.Application.Abstractions.Realtime;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Chats;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses.Events;

#pragma warning disable CA1303 // Do not pass literals as localized parameters
namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Handlers.Events;
internal sealed class MessageStatusReceivedDomainEventHandler(IServerEventChannel ServerEventChannel) 
    : IDomainEventHandler<MessageStatusReceivedDomainEvent>
{
    public async Task HandleAsync(MessageStatusReceivedDomainEvent @event, CancellationToken cancellationToken = default) 
        => await ServerEventChannel.BroadcastEventAsync(new(@event, "message-status-update"), cancellationToken);
}
