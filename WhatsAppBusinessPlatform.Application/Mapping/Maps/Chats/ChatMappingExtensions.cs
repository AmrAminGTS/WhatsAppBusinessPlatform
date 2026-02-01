using System.Linq.Expressions;
using LinqKit;
using System.Text.Json;
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
            Reactions = src.GetReactionsSummary,
            SentReaction = src.Reactions
                .Where(r => r.Direction == MessageDirection.Sent)
                .Select(r => new SentReaction( r.Emoji, r.Id.ToString()))
                .SingleOrDefault()
        };
        public RealTimeChatMessageResponse MapToRealTimeChatMessageResponse()
        {
            Func<WAMessage, ChatMessageResponse> func = WAMessage.ProjectToChatMessageResponse().Compile();
            ChatMessageResponse chatMessageResponse = func.Invoke(src);
            var realtimeCaht = new RealTimeChatMessageResponse(chatMessageResponse, src.ContactPhoneNumber);
            return realtimeCaht;
        }
    }
    extension(WAAccount src)
    {
        public static Expression<Func<WAAccount, ChatListItemDto>> ProjectToChatListItemDto(string userId) =>
            src => new ChatListItemDto
            {
                AccountType = AccountType.Normal,
                ContactId = src.Contact != null ? src.Contact.Id : null,
                PhoneNumber = src.PhoneNumber,
                FullName = src.Contact != null ? src.Contact.FirstName + " " + src.Contact.LastName : src.PhoneNumber,
                ContactType = ContactType.Individual,
                UnreadCount = src.Messages.Where(m => m.ContentType != MessageContentType.Reaction)
                    .Count(m => m.Direction == MessageDirection.Received
                             && m.MessageReaders.Any(mr => mr.UserId == userId) == false),

                LastUpdate = WAAccount.GetLastUpdateExpr().Invoke(src) // LinqKit will expand this
            };

        public static Expression<Func<WAAccount, ChatLastUpdateResponse?>> GetLastUpdateExpr() => account =>
                 // compare the max datetimes (coalesced to a safe min value)
                 ((account.Messages
                     .Where(m => m.ContentType != MessageContentType.Reaction)
                     .Max(m => (DateTimeOffset?)m.DateTimeOffset) ?? DateTimeOffset.MinValue)
                  >=
                  (account.Messages
                     .Where(m => m.ContentType != MessageContentType.Reaction)
                     .SelectMany(m => m.Reactions)
                     .Max(r => (DateTimeOffset?)r.DateTimeOffset) ?? DateTimeOffset.MinValue)
                 )
                 // if latest is a message -> take the latest message projection
                 ? account.Messages
                     .Where(m => m.ContentType != MessageContentType.Reaction)
                     .OrderByDescending(m => m.DateTimeOffset)
                     .Select(m => new ChatLastUpdateResponse
                     {
                         IsReaction = false,
                         When = m.DateTimeOffset,
                         Direction = m.Direction,
                         Message = new()
                         {
                             Id = m.Id,
                             ContentType = m.ContentType,
                             JsonContent = JsonSerializer.Deserialize<object>(m.JsonContent)!,
                             LastStatus = (m.Statuses.Count > 0 && m.Direction == MessageDirection.Sent)
                                 ? m.Statuses
                                     .OrderByDescending(s => s.DateTimeOffset)
                                     .ThenByDescending(s => s.Status)
                                     .Select(s => s.Status)
                                     .FirstOrDefault()
                                 : null
                         },
                         Reaction = null
                     })
                     .FirstOrDefault()
                 // else latest is a reaction -> take the latest reaction projection
                 : account.Messages
                     .SelectMany(m => m.Reactions)
                     .OrderByDescending(r => r.DateTimeOffset)
                     .Select(r => new ChatLastUpdateResponse
                     {
                         IsReaction = true,
                         When = r.DateTimeOffset,
                         Direction = r.Direction,
                         Message = null,
                         Reaction = new MessageReactionResponse(
                             r.Emoji,
                             JsonSerializer.Deserialize<object>(r.ReactedToMessage.JsonContent)!)
                     })
                     .FirstOrDefault();
    }
}
