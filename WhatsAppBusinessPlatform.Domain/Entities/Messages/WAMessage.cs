using System.Diagnostics.CodeAnalysis;
using WhatsAppBusinessPlatform.Domain.Common;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Domain.Entities.Messages;
public class WAMessage : BaseDomainEntity<string>
{
    // DateTime
    public DateTimeOffset DateTimeOffset { get; init; }
    // Type
    public MessageContentType ContentType { get; init; }
    // JsonContent
    public required byte[] JsonContent { get; init; }
    // Direction (sent/received)
    public MessageDirection Direction { get; init; }
    // BusinessPN_Id
    public required string BusinessPhoneId { get; init; }
    // ContactPhone (not specific to WA)
    public required string ContactPhoneNumber { get; init; }
    // Recipient_Type (individual/group)
    public ContactType RecipientType { get; init; } = ContactType.Individual;
    // If this message is a reply to another message
    public string? ReplyToId { get; init; }
    public required byte[]? RawWebhookReqest { get; init; }
    public required string? CreatedByUserId { get; init; }

    // Navigation Properties
    public required WAAccount WAAccount { get; init; }
    public WAMessage? ReplyTo { get; init; }
    public ICollection<WAMessage> Replies { get; init; } = [];
    public ICollection<MessageReaction> Reactions { get; init; } = [];
    public ICollection<MessageStatus> Statuses { get; init; } = [];
    public ICollection<MessageReader> MessageReaders { get; init; } = [];

    // Helpers
    public Dictionary<string, int> GetReactionsSummary
        => Reactions.GroupBy(mr => mr.Emoji).ToDictionary(g => g.Key, g => g.Count());

}
public enum ContactType
{
    Individual,
    Group
}
