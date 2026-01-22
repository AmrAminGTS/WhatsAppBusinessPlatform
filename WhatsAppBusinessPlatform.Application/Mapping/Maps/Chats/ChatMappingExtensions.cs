using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Application.Mapping.Maps.Chats;
internal static class ChatMappingExtensions
{
    extension(WAMessage src)
    {
        public static Expression<Func<WAMessage, ChatMessageResponse>> ProjectToChatMessageResponse() => src => new()
        {
            Id = src.Id,
            ContentType = src.ContentType,
            JsonContent = JsonSerializer.Deserialize<JsonElement>(src.JsonContent),
            Direction = src.Direction,
            DateTime = src.DateTimeOffset,
            LastStatus = src.Statuses.Any()
                ? src.Statuses.OrderByDescending(s => s.DateTimeOffset)
                    .ThenByDescending(s => s.Status)
                    .FirstOrDefault()!.Status
                : null,
            ReplyTo = src.ReplyTo != null
                ? new ReplyToMessageResponse()
                {
                    Id = src.ReplyTo.Id,
                    ContentType = src.ReplyTo.ContentType,
                    Direction = src.ReplyTo.Direction,
                    JsonContent = JsonSerializer.Deserialize<JsonElement>(src.ReplyTo.JsonContent),
                }
                : null,
            Reactions = src.Reactions.Select(r => new MessageReactionResponse(r.Emoji, r.Direction, r.UserId))
        };
        
        public RealTimeChatMessageResponse MapToRealTimeChatMessageResponse()
        {
            Func<WAMessage, ChatMessageResponse> func = WAMessage.ProjectToChatMessageResponse().Compile();
            ChatMessageResponse chatMessageResponse = func.Invoke(src);
            var realtimeCaht = new RealTimeChatMessageResponse(chatMessageResponse, src.ContactPhoneNumber);
            return realtimeCaht;
        }

        private static Expression<Func<WAMessage, ChatLastMessageResponse>> ProjectToChatLastMessageResponse() => (src) => new()
        {
            Id = src.Id,
            ContentType = src.ContentType,
            JsonContent = JsonSerializer.Deserialize<JsonElement>(src.JsonContent),
            Direction = src.Direction,
            DateTime = src.DateTimeOffset,
            LastStatus = src.Statuses.Count > 0 && src.Direction == MessageDirection.Sent
                        ? src.Statuses.OrderByDescending(s => s.DateTimeOffset)
                            .ThenByDescending(s => s.Status)
                            .FirstOrDefault()!.Status
                        : null,
        };
    }
    extension(WAAccount src)
    {
        public static Expression<Func<WAAccount, ChatListItemDto>> ProjectToChatListItemDto(string userId) => (src) => new ChatListItemDto
        {
            AccountType = AccountType.Normal,
            ContactId = src.Contact != null ? src.Contact.Id : null,
            PhoneNumber = src.PhoneNumber,
            FullName = src.Contact != null ? src.Contact.FirstName + " " + src.Contact.LastName : src.PhoneNumber,
            ContactType = ContactType.Individual,
            LastMessage = src.Messages.AsQueryable()
                .OrderByDescending(m => m.DateTimeOffset)
                .Select(WAMessage.ProjectToChatLastMessageResponse())
                .FirstOrDefault(m => m.ContentType != MessageContentType.Reaction ),

            UnreadCount = src.Messages.Where(m => m.ContentType != MessageContentType.Reaction)
                .Count(m => m.Direction == MessageDirection.Received
                && m.MessageReaders.Any(mr => mr.UserId == userId) == false),
        };
    }
}
