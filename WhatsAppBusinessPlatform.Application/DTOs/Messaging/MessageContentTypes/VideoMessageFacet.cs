using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageFacets;
internal sealed class VideoMessageFacet : MediaMessageFacet
{
    [JsonPropertyName("caption")]
    public required string Caption { get; set; }
}
