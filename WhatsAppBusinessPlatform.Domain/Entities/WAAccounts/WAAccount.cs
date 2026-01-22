using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Domain.Common;
using WhatsAppBusinessPlatform.Domain.Entities.Contacts;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

public class WAAccount : BaseDomainEntity<string>
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
    public required string PhoneNumber { get; set; }

    // Navigation Properties
    public Contact? Contact { get; set; }
    public ICollection<WAMessage> Messages { get; set; } = [];
}
