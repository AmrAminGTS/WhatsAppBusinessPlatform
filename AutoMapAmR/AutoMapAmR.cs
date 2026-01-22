using System.Reflection;
using AutoMapAmR.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AutoMapAmR;

// Auto-register
public static class AutoMapAmR
{
    public static IServiceCollection AddAutoMapAmR(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        services.AddScoped<IMapFinder, MapFinder>();

        // Register all IMap<,>, IMapExisting<,> implementations
        var mapperTypes = assembly.GetTypes()
            .Where(type => type.IsAbstract == false && type.IsInterface == false && type.IsGenericType == false)
            .SelectMany(type => type.GetInterfaces()
                .Where(i => i.IsGenericType 
                && (i.GetGenericTypeDefinition() == typeof(IMap<,>) 
                    || i.GetGenericTypeDefinition() == typeof(IMapExisting<,>)))
                .Select(i => new { mapperInterface = i, ImplementationType = type }));

        foreach (var mapperType in mapperTypes)
        {
            services.AddScoped(mapperType.mapperInterface, mapperType.ImplementationType);
        }

        return services;
    }
}

