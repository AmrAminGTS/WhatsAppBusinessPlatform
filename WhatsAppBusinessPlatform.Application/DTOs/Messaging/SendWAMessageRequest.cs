using System.Text.Json.Serialization;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;

namespace WhatsAppBusinessPlatform.Application.DTOs.Messaging;
public class SendWAMessageRequest<TMessageContent> where TMessageContent : IMessageContentType
{
    [JsonIgnore]
    public string BusinessPhoneNumberId  => "900551569815836";
    public required string RecipientPhoneNumber { get; set; }
    public string? ReplyToMessageId { get; set; } 
    public required TMessageContent MessageContent { get; set; }
}
public sealed class ReactToMessageRequest : SendWAMessageRequest<ReactionMessageContent>
{
    [JsonIgnore]
    public new string? ReplyToMessageId { get; set; } = null;
}
