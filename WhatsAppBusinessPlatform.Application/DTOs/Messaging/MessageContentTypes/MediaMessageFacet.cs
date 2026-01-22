using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageFacets;
internal class MediaMessageFacet
{
    [JsonPropertyName("id")]
    public required string Id { get; set; } //"<MEDIA_ID>", <!-- Only if using uploaded media -->

    // id or link (not both)

    [JsonPropertyName("link")] 
    public required string Link { get; set; } //"<MEDIA_URL>", <!-- Only if using hosted media(not recommended) -->

    public required IFormFile File { get; set; }
}
