using System.Text.Json.Serialization;

namespace WhatsAppBusinessPlatform.Infrastucture.ThirdParty.WACloudApi;

internal sealed class SendMessageErrorResponse
{
    [JsonPropertyName("error")]
    public required SendMessageError Error { get; set; }
}
