using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;
public sealed class ReactionMessageContent: IMessageContentType
{
    [JsonPropertyName("message_id")]
    public required string ReactedToMessageId { get; set; }

    [JsonPropertyName("emoji")]
    public string? Emoji { get; set; }
    MessageContentType IMessageContentType.MessageContentType => MessageContentType.Reaction;
}
