using MediatAmR.Abstractions;
using WhatsAppBusinessPlatform.Application.Abstractions.Realtime;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses.Events;

#pragma warning disable CA1303 // Do not pass literals as localized parameters
namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Handlers.Events;
internal sealed class MessageStatusReceivedDomainEventHandler(IRealTimeMessagingChannel realTimeMessagingChannel) 
    : IDomainEventHandler<MessageStatusReceivedDomainEvent>
{
    public async Task HandleAsync(MessageStatusReceivedDomainEvent @event, CancellationToken cancellationToken = default) 
        => await realTimeMessagingChannel.MessageStatusReceivedAsync(@event, cancellationToken);
}
