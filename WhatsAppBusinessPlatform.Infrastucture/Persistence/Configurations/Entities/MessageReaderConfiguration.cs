using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Infrastucture.Persistence.Configurations.Entities;

internal sealed class MessageReaderConfiguration : IEntityTypeConfiguration<MessageReader>
{
    public void Configure(EntityTypeBuilder<MessageReader> builder) 
        => builder.HasKey(c => new { c.UserId, c.MessageId });
}
