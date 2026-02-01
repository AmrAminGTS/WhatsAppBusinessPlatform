using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhatsAppBusinessPlatform.Domain.Common;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Infrastucture.Persistence.Configurations.Entities;
internal sealed class WAMessageConfiguration : IEntityTypeConfiguration<WAMessage>
{
    public void Configure(EntityTypeBuilder<WAMessage> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.ContactPhoneNumber);

        builder.HasMany(e => e.Statuses)
            .WithOne(s => s.Message)
            .HasForeignKey(f => f.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Replies)
            .WithOne(f => f.ReplyTo)
            .HasForeignKey(f => f.ReplyToId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Reactions)
            .WithOne(f => f.ReactedToMessage)
            .HasForeignKey(f => f.ReactedToMessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.MessageReaders)
            .WithOne(f => f.Message)
            .HasForeignKey(f => f.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.Id).HasMaxLength(DomainConstraints.MessageIdMaxLength);
        builder.Property(e => e.BusinessPhoneId).HasMaxLength(DomainConstraints.ShortString.Max);
        builder.Property(e => e.ContactPhoneNumber).HasMaxLength(DomainConstraints.ShortString.Max);
        builder.Property(e => e.ReplyToId).HasMaxLength(DomainConstraints.MessageIdMaxLength);
        builder.Property(e => e.CreatedByUserId).HasMaxLength(DomainConstraints.MediumString.Max);
    }
}
