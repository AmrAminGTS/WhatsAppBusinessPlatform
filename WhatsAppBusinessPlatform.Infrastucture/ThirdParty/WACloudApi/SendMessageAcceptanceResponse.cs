using System.Text.Json.Serialization;

namespace WhatsAppBusinessPlatform.Infrastucture.ThirdParty.WACloudApi;

internal sealed class SendMessageAcceptanceResponse
{
    [JsonPropertyName("messaging_product")]
    public required string MessagingProduct { get; set; }
    [JsonPropertyName("contacts")]
    public required ICollection<ContactInfo> Contacts { get; set; }
    [JsonPropertyName("messages")]
    public required ICollection<SentMessageStatus> Messages { get; set; }
}
