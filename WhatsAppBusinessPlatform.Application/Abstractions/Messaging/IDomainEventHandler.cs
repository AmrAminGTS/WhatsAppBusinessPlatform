using MediatAmR.Abstractions;

namespace WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
public interface IDomainEventHandler<in T> where T : IEvent
{
    Task Handle(T domainEvent, CancellationToken cancellationToken);
}
