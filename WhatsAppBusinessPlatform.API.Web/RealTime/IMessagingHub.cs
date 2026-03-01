using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses.Events;

namespace WhatsAppBusinessPlatform.API.Web.RealTime;

public interface IMessagingHub
{
    Task MessageReceived(RealTimeChatMessageResponse message, CancellationToken cancellationToken);
    Task MessageStatusReceived(MessageStatusReceivedDomainEvent messageStatus, CancellationToken cancellationToken);
    Task MessageReactionsChanged(MessageReactionsChangedResponse messageReactions, CancellationToken cancellationToken);
}
