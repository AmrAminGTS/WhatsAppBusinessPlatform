using System.Text.Json.Serialization;

namespace WhatsAppBusinessPlatform.Infrastucture.ThirdParty.WACloudApi;

internal sealed class ContactInfo
{
    [JsonPropertyName("input")]
    public required string Input { get; set; }
    [JsonPropertyName("wa_id")]
    public required string WaId { get; set; }
}
