using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageFacets;
internal sealed class ImageMessageFacet : MediaMessageFacet
{
    [JsonPropertyName("caption")]
    public string? Caption { get; set; }
}
