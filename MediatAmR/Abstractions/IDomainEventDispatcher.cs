using System;
using System.Collections.Generic;
using System.Text;

namespace MediatAmR.Abstractions;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IEvent> domainEvents, CancellationToken cancellationToken = default);
}
