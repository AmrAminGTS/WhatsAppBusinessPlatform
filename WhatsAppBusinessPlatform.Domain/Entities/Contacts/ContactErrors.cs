using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Domain.Common;

namespace WhatsAppBusinessPlatform.Domain.Entities.Contacts;
public static class ContactErrors
{
    public static DomainError NotFound(string id) => DomainError.NotFound("Contact.NotFound", id);
    public static DomainError AlreadyExists(string phone) => DomainError.Conflict("Contact.AlreadyExists", phone);
}
