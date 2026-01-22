using System;
using System.Collections.Generic;
using System.Text;

namespace MediatAmR.Abstractions;

public interface IDomainEventHandler<in TEvent>
    where TEvent : IEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
