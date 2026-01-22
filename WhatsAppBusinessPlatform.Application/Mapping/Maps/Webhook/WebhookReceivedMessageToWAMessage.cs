using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using Azure.Core;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;
using WhatsAppBusinessPlatform.Application.DTOs.Webhook;
using WhatsAppBusinessPlatform.Application.Mapping;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Application.Mapping.Maps.Webhook;
internal static class WAMessageMappingExtensions
{
    public static WAMessage MapToWAMessage(this WebhookReceivedMessage src, WAAccount wAAccount, string businessPhoneId, byte[]? rawMessageBytes = null)
    {
        var contentType = MessageContentType.FromString(src.MessageType);
        return new()
        {
            Id = src.MessageId,
            WAAccount = wAAccount,
            DateTimeOffset = DateTimeOffset.FromTimestampString(src.Timestamp),
            ContentType = contentType,
            JsonContent = src.JsonContent!,
            Direction = MessageDirection.Received,
            ContactPhoneNumber = src.FromPhoneNumber,
            ReplyToId = src.ReplayTo?.MessageId,
            BusinessPhoneId = businessPhoneId,
            RawWebhookReqest = rawMessageBytes,
            CreatedByUserId = null
        }; 
    }
    public static WAMessage MapToWAMessage<TMessageContent>(
        this SendWAMessageRequest<TMessageContent> src,
        string messageId,
        WAAccount wAAccount,
        string creatorUserId,
        DateTimeOffset sentAt) where TMessageContent : IMessageContentType => new()
        {
            Id = messageId,
            WAAccount = wAAccount,
            DateTimeOffset = sentAt,
            ContentType = src.MessageContent.MessageContentType,
            JsonContent = JsonSerializer.SerializeToUtf8Bytes(src.MessageContent),
            Direction = MessageDirection.Sent,
            BusinessPhoneId = src.BusinessPhoneNumberId,
            ContactPhoneNumber = src.RecipientPhoneNumber,
            ReplyToId = src.ReplyToMessageId,
            RawWebhookReqest = null,
            CreatedByUserId = creatorUserId
        };
}
