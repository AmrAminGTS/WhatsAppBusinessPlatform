using System.Linq.Expressions;

namespace WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
public interface IRepository<T> where T : class
{
    // Commands
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);

    // Queries
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T?> FirstOrDefaultAsync();
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> ToListAsync(CancellationToken cancellationToken = default);
    IQueryable<T> Where(Expression<Func<T, bool>> predicate);
    IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> expression);
    Task<T> GetOrAddAsync(Expression<Func<T, bool>> expression, T entity, CancellationToken cancellationToken = default);
    Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);
}
