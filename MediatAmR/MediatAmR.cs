using Microsoft.Extensions.DependencyInjection;
using MediatAmR.Abstractions;
using System.Reflection;

namespace MediatAmR;

public static class MediatAmR
{
    public static IServiceCollection AddMediatAmR(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        services.AddScoped<IRequestSender, RequestSender>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        
        // Register all IRequestHandler and IDomainEventHandler implementations
        var handlerTypes = assembly.GetTypes()
            .Where(type => type.IsAbstract == false && type.IsInterface == false && type.IsGenericType == false)
            .SelectMany(type => type.GetInterfaces()
                .Where(i => i.IsGenericType 
                    && (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
                    || i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)))
                .Select(i => new { HandlerInterface = i, ImplementationType = type }));

        foreach (var handlerType in handlerTypes)
        {
            services.AddScoped(handlerType.HandlerInterface, handlerType.ImplementationType);
        }

        return services;
    }
}
