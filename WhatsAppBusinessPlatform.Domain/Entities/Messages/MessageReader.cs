using WhatsAppBusinessPlatform.Domain.Common;

namespace WhatsAppBusinessPlatform.Domain.Entities.Messages;

public class MessageReader
{
    public required string UserId { get; set; }
    public required string MessageId { get; set; }

    // Navigation Properties
    public WAMessage? Message { get; set; }
}
