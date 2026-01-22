using System.Text.Json.Serialization;

namespace WhatsAppBusinessPlatform.Infrastucture.ThirdParty.WACloudApi;

internal sealed class SentMessageStatus
{
    [JsonPropertyName("id")]
    public required string MessageId { get; set; }

    [JsonPropertyName("message_status")]
    public string? MessageStatus { get; set; }
}
