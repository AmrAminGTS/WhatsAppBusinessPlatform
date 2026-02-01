using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses;

namespace WhatsAppBusinessPlatform.Application.DTOs.Chats;

public sealed class GetAllChatsResponse
{
    public required IListWithPaginationInfo<ChatListItemDto> Chats { get; set; } 
}

public sealed class ChatListItemDto
{
    public AccountType AccountType { get; set; } 
    public string? ContactId { get; set; }
    public string? FullName { get; set; } 
    public required string PhoneNumber { get; set; } 
    public ContactType ContactType { get; set; } 
    public ChatLastUpdateResponse? LastUpdate { get; set; }
    public required int UnreadCount { get; set; }
}

public sealed class ChatLastMessageResponse
{
    public required string Id { get; set; }
    public required MessageContentType ContentType { get; init; }
    public required object JsonContent { get; init; }
    public required MessageStatusType? LastStatus { get; init; }
}
public class ChatLastUpdateResponse
{
    public required bool IsReaction { get; init; }
    public required ChatLastMessageResponse? Message { get; init; } 
    public required MessageReactionResponse? Reaction { get; init; }
    public required DateTimeOffset When { get; init; } 
    public required MessageDirection Direction { get; init; }
}

public enum AccountType
{
    Normal,
    Business
}
