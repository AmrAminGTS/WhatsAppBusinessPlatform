using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhatsAppBusinessPlatform.Domain.Common;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Infrastucture.Persistence.Configurations.Entities;

internal sealed class MessageReactionConfiguration : IEntityTypeConfiguration<MessageReaction>
{
    public void Configure(EntityTypeBuilder<MessageReaction> builder)
    {
        builder.HasKey(mr => mr.Id);
        builder.Property(mr => mr.Emoji).HasMaxLength(DomainConstraints.ShortString.Max);
        builder.Property(mr => mr.ReactedToMessageId).HasMaxLength(DomainConstraints.MediumString.Max);
        builder.Property(mr => mr.UserId).HasMaxLength(DomainConstraints.MediumString.Max);
    }
}
