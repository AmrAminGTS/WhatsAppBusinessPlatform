using System.Collections.ObjectModel;
using MediatAmR.Abstractions;

namespace WhatsAppBusinessPlatform.Domain.Common;
public abstract class BaseDomainEntity<TKey>
{
    public virtual TKey Id { get; set; } = default!;
    public bool IsDeleted { get; set; }

    // Event Methods
    private readonly List<IEvent> _domainEvents = [];
    public IReadOnlyCollection<IEvent> DomainEvents => [.. _domainEvents];
    public void ClearDomainEvents() => _domainEvents.Clear();
    public void Raise(IEvent domainEvent) => _domainEvents.Add(domainEvent);
}
