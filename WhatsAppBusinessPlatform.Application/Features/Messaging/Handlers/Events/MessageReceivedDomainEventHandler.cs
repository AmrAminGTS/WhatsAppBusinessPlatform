using System;
using System.Collections.Generic;
using System.Net.ServerSentEvents;
using System.Text;
using System.Threading.Channels;
using MediatAmR.Abstractions;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Abstractions.Realtime;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Events;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Chats;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

#pragma warning disable CA1303 // Do not pass literals as localized parameters
namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Handlers.Events;
internal sealed class MessageReceivedDomainEventHandler(IServerEventChannel ServerEventChannel) 
    : IDomainEventHandler<MessageReceivedDomainEvent>
{
    public async Task HandleAsync(MessageReceivedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"New Message Saved: {@event.Message.Id}");
        Console.ResetColor();

        if (@event.Message.ContentType == MessageContentType.Reaction)
        {
            // To Do
        }
        RealTimeChatMessageResponse payload = @event.Message.MapToRealTimeChatMessageResponse();
        await ServerEventChannel.BroadcastEventAsync(new(payload, "message-received"), cancellationToken);
    }
}
