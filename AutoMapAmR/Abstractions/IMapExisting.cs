namespace AutoMapAmR.Abstractions;

public interface IMapExisting<TSource, TDestination>
{
    Func<TSource, TDestination, TDestination> MapFunc { get; }
    TDestination Map(TSource source, TDestination destination)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);
        Func<TSource, TDestination, TDestination> projector = MapFunc;
        return projector(source, destination);
    }
}

