using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Domain.Entities.Messages;

public class MessageReaction
{
    public long Id { get; set; }
    public string? UserId
    {
        get; set => field = Direction == MessageDirection.Sent 
            ? value 
            : throw new ArgumentException("Only sent reaction should have UserId");
    }
    public required string Emoji { get; set; }
    public required DateTimeOffset DateTimeOffset { get; set; }
    public required MessageDirection Direction { get; set; }
    public required string ReactedToMessageId { get; set; }

    // Navigation Properties
    public required WAMessage ReactedToMessage { get; set; }
    public string? ReactedByAccountId
    {
        get; set => field = Direction == MessageDirection.Received 
            ? value 
            : throw new ArgumentException("Only received reaction should have Account");
    }
    public WAAccount? ReactedByAccount
    {
        get; set => field = Direction == MessageDirection.Received 
            ? value 
            : throw new ArgumentException("Only received reaction should have Account");
    }

#pragma warning disable S3358 // Ternary operators should not be nested
    public string? ContactFullName => Direction == MessageDirection.Received
                            ? (ReactedByAccount!.Contact != null
                                    ? ReactedByAccount.Contact.FullName
                                    : ReactedByAccount.PhoneNumber)
                            : null;
#pragma warning restore S3358 // Ternary operators should not be nested

}
