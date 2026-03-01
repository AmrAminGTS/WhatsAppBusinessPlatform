using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses.Events;

namespace WhatsAppBusinessPlatform.Application.Abstractions.Realtime;

public interface IRealTimeMessagingChannel
{
    Task MessageReceivedAsync(RealTimeChatMessageResponse message, CancellationToken cancellationToken);
    Task MessageStatusReceivedAsync(MessageStatusReceivedDomainEvent messageStatus, CancellationToken cancellationToken);
    Task MessageReactionsChangedAsync(MessageReactionsChangedResponse messageReactions, CancellationToken cancellationToken);
}
