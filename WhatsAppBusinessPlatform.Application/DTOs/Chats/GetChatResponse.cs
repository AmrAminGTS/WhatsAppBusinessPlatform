using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses;

namespace WhatsAppBusinessPlatform.Application.DTOs.Chats;

public sealed class GetChatResponse
{
    public AccountType AccountType { get; init; } = AccountType.Normal;
    public required IListWithPaginationInfo<ChatMessageResponse> ChatMessages { get; init; }
}

public class  BaseChatMessageResponse
{
    public required string Id { get; init; }
    public required MessageContentType ContentType { get; init; }
    public required JsonElement JsonContent { get; init; }
    public required MessageDirection Direction { get; init; }
}
public class ChatMessageResponse : BaseChatMessageResponse
{
    public required DateTimeOffset DateTime { get; init; }
    public MessageStatusType? LastStatus { get; init; }
    public ReplyToMessageResponse? ReplyTo{ get; init; }
    public IDictionary<string, int> Reactions { get; init; } = new Dictionary<string, int>();
    public string? SentReaction { get; set; } 
}
public sealed class RealTimeChatMessageResponse : ChatMessageResponse
{
    [SetsRequiredMembers]
    public RealTimeChatMessageResponse(ChatMessageResponse src, string contactPhoneNumber)
    {
        DateTime = src.DateTime;
        LastStatus = src.LastStatus;
        ReplyTo = src.ReplyTo;
        Id = src.Id;
        ContentType = src.ContentType;
        JsonContent = src.JsonContent;
        Direction = src.Direction;
        ContactPhoneNumber = contactPhoneNumber;
    }
    public required string ContactPhoneNumber { get; set; }
}

public sealed class ReplyToMessageResponse : BaseChatMessageResponse;
public sealed record MessageReactionResponse(
    string Emoji,
    MessageDirection Direction,
    string? UserId,
    string? ContactFullName,
    object ReactedToMessage);
