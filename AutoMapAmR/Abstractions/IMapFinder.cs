using System.Linq.Expressions;

namespace AutoMapAmR.Abstractions;

public interface IMapFinder
{
    Expression<Func<TSource, TDestination>> Projector<TSource, TDestination>();
    TDestination MapTo<TDestination>(object source);
    TDestination MapTo<TSource, TDestination>(TSource source, TDestination destination);
}
