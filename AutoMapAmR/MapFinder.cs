using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using AutoMapAmR.Abstractions;

namespace AutoMapAmR;
public sealed class MapFinder : IMapFinder
{
    private readonly IServiceProvider _serviceProvider;
    public MapFinder(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public TDestination MapTo<TDestination>(object source)
    {
        Type mapType = typeof(IMap<,>).MakeGenericType(source!.GetType(), typeof(TDestination));
        return InvokeMethodOn<TDestination>(mapType, "Map", source);
    }
    public TDestination MapTo<TSource, TDestination>(TSource source, TDestination destination)
    {
        Type mapType = typeof(IMapExisting<,>).MakeGenericType(typeof(TSource), typeof(TDestination));
        return InvokeMethodOn<TDestination>(mapType, "Map", source, destination);
    }
    public Expression<Func<TSource, TDestination>> Projector<TSource, TDestination>()
    {
        Type mapType = typeof(IMap<,>).MakeGenericType(typeof(TSource), typeof(TDestination));
        object map = _serviceProvider
            .GetService(mapType) ?? throw new InvalidOperationException($"Map of type '{mapType}' not found.");

        PropertyInfo? mapExpressionProperty = mapType.GetProperty("MapExpression");
        ArgumentNullException.ThrowIfNull(mapExpressionProperty);
        return (Expression<Func<TSource, TDestination>>)mapExpressionProperty.GetValue(map)!;
    }

    // Private methods
    private TResult InvokeMethodOn<TResult>(Type type, string methodName, params object?[] parameters)
    {
        object map = _serviceProvider
            .GetService(type) ?? throw new InvalidOperationException($"Map of type '{type}' not found.");

        MethodInfo? mapToMethod = type.GetMethod(methodName);
        ArgumentNullException.ThrowIfNull(mapToMethod);
        return (TResult)mapToMethod.Invoke(map, parameters)!;
    }
}
