using System.Collections.Generic;
using System.Collections.ObjectModel;
using MediatAmR.Abstractions;
using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Domain.Common;
using WhatsAppBusinessPlatform.Domain.Entities.Contacts;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Infrastucture.Persistence;
public class ApplicationDbContext : DbContext
{
    private readonly IDomainEventDispatcher _eventDispatcher;
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDomainEventDispatcher eventDispatcher) : base(options) => _eventDispatcher = eventDispatcher;
    public DbSet<WAMessage> Messages { get; set; }
    public DbSet<MessageStatus> MessageStatuses { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<WAAccount> Accounts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);
        await PublishDomainEventsAsync();
        return result;
    }

    // Private Methods
    private async Task PublishDomainEventsAsync()
    {
        // Get all entities with domain events
        var domainEvents = ChangeTracker
            .Entries<BaseDomainEntity<string>>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyCollection<IEvent> domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();
                return domainEvents;
            }).ToList();

        try
        {
            await _eventDispatcher.DispatchAsync(domainEvents);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("One or more domain events could not be Dispatched.", ex);
        }
    }
}
