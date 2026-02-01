using System.ComponentModel.DataAnnotations.Schema;
using WhatsAppBusinessPlatform.Domain.Common;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Domain.Entities.Messages;

public class MessageReaction : BaseDomainEntity<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public required string Emoji { get; set; }
    public required DateTimeOffset DateTimeOffset { get; set; }
    public required MessageDirection Direction { get; set; }
    public required string MessageId { get; set; }
    public required string ReactedToMessageId { get; set; }
    public string? UserId
    {
        get; set => field = (Direction == MessageDirection.Sent && value != null 
            || Direction == MessageDirection.Received && value == null)
            ? value 
            : throw new ArgumentException($"Sent reaction should have {nameof(UserId)}");
    }

    // Navigation Properties
    public required WAMessage ReactedToMessage { get; set; }
    public required WAMessage Message { get; set; }
    public ICollection<MessageStatus> Statuses { get; set; } = [];
    public string? ContactAccountId
    {
        get; set => field = (Direction == MessageDirection.Received && value != null
            || Direction == MessageDirection.Sent && value == null)
            ? value 
            : throw new ArgumentException($"Received reaction should have {nameof(ContactAccountId)}");
    }
    public WAAccount? ContactAccount
    {
        get; set => field = (Direction == MessageDirection.Received && value != null
            || Direction == MessageDirection.Sent && value == null)
            ? value 
            : throw new ArgumentException($"Received reaction should have {nameof(ContactAccount)}");
    }
}
