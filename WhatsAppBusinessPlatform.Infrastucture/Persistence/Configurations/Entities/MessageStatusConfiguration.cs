using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses;

namespace WhatsAppBusinessPlatform.Infrastucture.Persistence.Configurations.Entities;

internal sealed class MessageStatusConfiguration : IEntityTypeConfiguration<MessageStatus>
{
    public void Configure(EntityTypeBuilder<MessageStatus> builder) 
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(p => p.Error)
            .WithOne(f => f.Status)
            .HasForeignKey<StatusError>(p => p.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
