using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Domain.Common;
using WhatsAppBusinessPlatform.Infrastucture.Persistence.Repositories;

namespace WhatsAppBusinessPlatform.Infrastucture.Persistence;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Dictionary<Type, object> _repositories = [];
    private readonly ILogger<UnitOfWork>? _logger;
    public UnitOfWork(ApplicationDbContext context, ILogger<UnitOfWork>? logger)
    {
        _context = context;
        _logger = logger;
    }

    public IRepository<T> Repository<T>() where T : class
    {
        Type type = typeof(T);
        if (!_repositories.TryGetValue(type, out object? value))
        {
            var repoInstance = new Repository<T>(_context);
            value = repoInstance;
            _repositories[type] = value;
        }
        return (IRepository<T>)value;
    }
    public IExecutionStrategy CreateExecutionStrategy() => _context.Database.CreateExecutionStrategy();
    public async Task<IDbContextTransaction> BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
    public async Task<Result<int>> SaveAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "An error occurred while saving changes to the database.");
            return GeneralErrors.FailureWhileSavingToDatabase();
        }
    }

    // Custom Repositories
    public IMessageReactionRepository ReactionRepository => field ??= new MessageReactionRepository(_context);
    public IWAAccountRepository AccountRepository => field ??= new WAAccountRepository(_context);
}
