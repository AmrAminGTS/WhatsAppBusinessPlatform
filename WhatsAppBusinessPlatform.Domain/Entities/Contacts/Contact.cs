using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Domain.Common;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Domain.Entities.Contacts;
public sealed class Contact : BaseDomainEntity<string>
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string CreatedBy { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required string WAAcountId { get; set; }

    // Navigation properties
    public required WAAccount WAAccount { get; set; }

    // Helpers
    public string FullName => string.Join(" ", FirstName, LastName);
}
