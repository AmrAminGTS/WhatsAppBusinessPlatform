using System.Text.Json;
using System.Text.Json.Serialization;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Application.DTOs.Webhook;
internal sealed class ReceivedUpdate
{
    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("entry")]
    public required ICollection<Entry> Entry { get; set; }
}
internal sealed class Entry
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("changes")]
    public required ICollection<Change> Changes { get; set; }
}
internal sealed class Change
{
    [JsonPropertyName("value")]
    public required Value Value { get; set; }

    [JsonPropertyName("field")]
    public required string Field { get; set; }
}
internal sealed class Value
{
    [JsonPropertyName("messaging_product")]
    public required string MessagingProduct { get; set; }

    [JsonPropertyName("metadata")]
    public required WebhookMetadata Metadata { get; set; }

    [JsonPropertyName("contacts")]
    public ICollection<ContactInfo>? Contacts { get; set; }

    [JsonPropertyName("statuses")]
    public ICollection<WebhookReceivedMessageStatus>? Statuses { get; set; }

    [JsonPropertyName("messages")]
    public ICollection<WebhookReceivedMessage>? Messages { get; set; }
}
internal sealed class WebhookMetadata
{
    [JsonPropertyName("display_phone_number")]
    public required string DisplayPhoneNumber { get; set; }

    [JsonPropertyName("phone_number_id")]
    public required string PhoneNumberId { get; set; }
}
internal sealed class WebhookReceivedMessageStatus
{
    [JsonPropertyName("id")]
    public required string MessageId { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("timestamp")]
    public required string Timestamp { get; set; }

    [JsonPropertyName("recipient_id")]
    public required string RecipientId { get; set; }

    [JsonPropertyName("pricing")]
    public Pricing? Pricing { get; set; }

    [JsonPropertyName("errors")]
    public ICollection<ReceivedStatusError>? Errors { get; set; }
}
internal sealed class Pricing
{
    [JsonPropertyName("billable")]
    public bool Billable { get; set; }

    [JsonPropertyName("pricing_model")]
    public required string PricingModel { get; set; }

    [JsonPropertyName("category")]
    public required string Category { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }
}
internal sealed class WebhookReceivedMessage
{
    [JsonPropertyName("context")]
    public Context? ReplayTo { get; set; }

    [JsonPropertyName("from")]
    public required string FromPhoneNumber { get; set; }

    [JsonPropertyName("id")]
    public required string MessageId { get; set; }

    [JsonPropertyName("timestamp")]
    public required string Timestamp { get; set; }

    [JsonPropertyName("type")]
    public required string MessageType { get; set; }

    // captures all unknown properties (including "text", "sticker", ...)
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? Extra { get; set; }

    // helper to easily get the content for the current Type
    [JsonIgnore]
    public byte[]? JsonContent
    {
        get
        {
            if (Extra != null && MessageType != null && Extra.TryGetValue(MessageType, out JsonElement element))
            {
                return JsonSerializer.SerializeToUtf8Bytes(element);
            }

            return null;
        }
    }
}
internal sealed class Context
{
    [JsonPropertyName("from")]
    public required string From { get; set; }

    [JsonPropertyName("id")]
    public required string MessageId { get; set; }
}
internal sealed class ContactInfo
{
    [JsonPropertyName("profile")]
    public required ContactProfile Profile { get; set; }

    [JsonPropertyName("wa_id")]
    public required string WAId { get; set; }
}
internal sealed class ContactProfile
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}
internal sealed class ReceivedStatusError
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }

    [JsonPropertyName("error_data")]
    public required ErrorData ErrorData { get; set; }

    [JsonPropertyName("href")]
    public required string Href { get; set; }
}
internal sealed class ErrorData
{
    [JsonPropertyName("details")]
    public required string Details { get; set; }
}
