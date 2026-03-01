using Microsoft.AspNetCore.SignalR;
using WhatsAppBusinessPlatform.Application.Abstractions.Realtime;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses.Events;

namespace WhatsAppBusinessPlatform.API.Web.RealTime;
internal sealed class RealTimeMessagingChannel(IHubContext<MessagingHub, IMessagingHub> messagingHub) : IRealTimeMessagingChannel
{
    public async Task MessageReactionsChangedAsync(MessageReactionsChangedResponse messageReactions, CancellationToken cancellationToken) 
        => await messagingHub.Clients.All.MessageReactionsChanged(messageReactions, cancellationToken);
    public async Task MessageReceivedAsync(RealTimeChatMessageResponse message, CancellationToken cancellationToken) 
        => await messagingHub.Clients.All.MessageReceived(message, cancellationToken);
    public async Task MessageStatusReceivedAsync(MessageStatusReceivedDomainEvent messageStatus, CancellationToken cancellationToken) 
        => await messagingHub.Clients.All.MessageStatusReceived(messageStatus, cancellationToken);
}
