using System.Text.Json.Serialization;

namespace WhatsAppBusinessPlatform.Infrastucture.ThirdParty.WACloudApi;

internal sealed class SendMessageError
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
    [JsonPropertyName("type")]
    public required string Type { get; set; }
    [JsonPropertyName("code")]
    public required int Code { get; set; }
    [JsonPropertyName("fbtrace_id")]
    public required string TraceId { get; set; }
}
