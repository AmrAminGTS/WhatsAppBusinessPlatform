using System;
using System.Collections.Generic;
using System.Text;

namespace WhatsAppBusinessPlatform.Application.DTOs.Contacts;

public class CommonContactDto
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string PhoneNumber { get; set; }
}
