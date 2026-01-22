using System.Linq.Expressions;

namespace AutoMapAmR.Abstractions;

public interface IMap<TSource, TDestination>
{
    Expression<Func<TSource, TDestination>> MapExpression { get; }
    TDestination Map(TSource source)
    {
        ArgumentNullException.ThrowIfNull(source);
        Func<TSource, TDestination> projector = MapExpression.Compile();
        return projector(source);
    }
}

