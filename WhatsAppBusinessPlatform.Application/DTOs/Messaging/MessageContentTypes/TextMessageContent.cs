using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;
public sealed class TextMessageContent: IMessageContentType
{
    [JsonPropertyName("body")]
    public required string Body { get; set; }

    [JsonPropertyName("preview_url")]
    public bool PreviewUrl { get; set; } = true;
    MessageContentType IMessageContentType.MessageContentType => MessageContentType.Text;
}
