using System;
using System.Collections.Generic;
using System.Text;

namespace WhatsAppBusinessPlatform.Application.DTOs.Contacts;

public sealed class GetAllContactsResponse 
{
    public required string Id { get; set; }
    public required string FullName { get; set; }
}
