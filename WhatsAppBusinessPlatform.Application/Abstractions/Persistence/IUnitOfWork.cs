using Microsoft.EntityFrameworkCore.Storage;
using WhatsAppBusinessPlatform.Application.Common;

namespace WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
public interface IUnitOfWork
{
    IMessageReactionRepository ReactionRepository { get; }
    IWAAccountRepository AccountRepository { get; }

    Task<Result<int>> SaveAsync(CancellationToken cancellationToken = default);
    IExecutionStrategy CreateExecutionStrategy();
    Task<IDbContextTransaction> BeginTransactionAsync();
    IRepository<T> Repository<T>() where T : class;
}
