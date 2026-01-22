using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageFacets;
internal sealed class ContactsMessageFacet
{
    [JsonPropertyName("contacts")]
    public required ICollection<ContactMessageItem> Contacts { get; set; }
}
internal sealed class ContactMessageItem
{
    [JsonPropertyName("name")]
    public required ContactName Name { get; set; }

    [JsonPropertyName("birthday")]
    public string? Birthday { get; set; }

    // Collections
    [JsonPropertyName("emails")]
    public ICollection<ContactEmail> Emails { get; set; } = [];

    [JsonPropertyName("org")]
    public Organization? Org { get; set; }

    [JsonPropertyName("phones")]
    public ICollection<ContactPhone> Phones { get; set; } = [];

    [JsonPropertyName("urls")]
    public ICollection<ContactUrl> Urls { get; set; } = [];

    [JsonPropertyName("addresses")]
    public ICollection<Address> Addresses { get; set; } = [];
}
internal sealed class ContactName
{
    [JsonPropertyName("formatted_name")] 
    public string? FormattedName { get; set; }

    [JsonPropertyName("first_name")] 
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")] 
    public string? LastName { get; set; }

    [JsonPropertyName("middle_name")] 
    public string? MiddleName { get; set; }

    [JsonPropertyName("suffix")] 
    public string? Suffix { get; set; }

    [JsonPropertyName("prefix")]
    public string? Prefix { get; set; }
}
internal sealed class Organization
{
    [JsonPropertyName("company")]
    public string? Company { get; set; }

    [JsonPropertyName("Department")]
    public string? Department { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }
}
internal sealed class Address
{
    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("zip")]
    public string? Zip { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("country_code")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
internal sealed class ContactEmail
{
    [JsonPropertyName("Email")]
    public string? Email { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
internal sealed class ContactPhone
{
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("wa_id")]
    public string? WAId { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
internal sealed class ContactUrl
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
