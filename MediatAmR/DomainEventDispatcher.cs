using System.Reflection;
using MediatAmR.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MediatAmR;

public sealed class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(domainEvents);
        foreach (IEvent domainEvent in domainEvents)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            Type handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType) 
                ?? throw new InvalidOperationException($"Handler of type '{handlerType}' not found.");

            foreach (object? handler in handlers)
            {
                if (handler is null)
                {
                    continue;
                }
                MethodInfo? handleMethod = handlerType.GetMethod("HandleAsync");
                ArgumentNullException.ThrowIfNull(handleMethod);
                await (Task)handleMethod.Invoke(handler, [domainEvent, cancellationToken])!;
            }
        }
    }
}
