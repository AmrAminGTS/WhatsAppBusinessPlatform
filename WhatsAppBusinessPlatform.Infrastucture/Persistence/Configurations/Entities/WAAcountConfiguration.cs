using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhatsAppBusinessPlatform.Domain.Common;
using WhatsAppBusinessPlatform.Domain.Entities.Contacts;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Infrastucture.Persistence.Configurations.Entities;

internal sealed class WAAcountConfiguration : IEntityTypeConfiguration<WAAccount>
{
    public void Configure(EntityTypeBuilder<WAAccount> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => new { c.PhoneNumber })
            .IsUnique();

        builder.HasMany(p => p.Messages)
            .WithOne(f => f.WAAccount)
            .HasPrincipalKey(c => c.PhoneNumber)
            .HasForeignKey(m => m.ContactPhoneNumber)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Contact)
            .WithOne(f => f.WAAccount)
            .HasForeignKey<Contact>(f =>f.WAAcountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.PhoneNumber).HasMaxLength(DomainConstraints.ShortString.Max);
    }
}
