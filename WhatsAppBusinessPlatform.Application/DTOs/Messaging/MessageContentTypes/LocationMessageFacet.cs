using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageFacets;
internal sealed class LocationMessageFacet
{
    [JsonPropertyName("latitude")]
    public required string Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public required string Longitude { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }
}
