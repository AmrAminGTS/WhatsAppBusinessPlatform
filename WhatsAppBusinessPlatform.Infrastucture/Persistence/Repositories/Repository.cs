using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Domain.Common;

namespace WhatsAppBusinessPlatform.Infrastucture.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    public Repository(ApplicationDbContext context) => _context = context;

    // Commands
    public void Add(T entity) => _context.Add(entity);
    public void AddRange(IEnumerable<T> entities) => _context.AddRange(entities);
    public void Update(T entity) => _context.Update(entity);
    public void UpdateRange(IEnumerable<T> entities) => _context.UpdateRange(entities);
    public void Delete(T entity) => _context.Remove(entity);

    // Queries
    public async Task<T?> FirstOrDefaultAsync() => await _context.Set<T>().FirstOrDefaultAsync();
    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _context.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) 
        => await _context.Set<T>().AnyAsync(predicate, cancellationToken);
    public async Task<IReadOnlyList<T>> ToListAsync(CancellationToken cancellationToken = default) 
        => await _context.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    public IQueryable<T> Where(Expression<Func<T, bool>> predicate) 
        => _context.Set<T>().Where(predicate);
    public IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> expression)
        => _context.Set<T>().Select(expression);
    public async Task<T> GetOrAddAsync(Expression<Func<T, bool>> expression, T entity, CancellationToken cancellationToken = default)
    {
        DbSet<T> entitySet = _context.Set<T>();
        T? existing = await entitySet.Where(expression).SingleOrDefaultAsync(cancellationToken);
        if(existing == null)
        {
            entitySet.Add(entity);
            return entity;
        }
        return existing;
    }
    public IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        => _context.Set<T>().Include(navigationPropertyPath);
}
