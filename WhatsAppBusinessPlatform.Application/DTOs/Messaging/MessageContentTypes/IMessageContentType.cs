using System.Text.Json.Serialization;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;

public interface IMessageContentType
{
    [JsonIgnore]
    MessageContentType MessageContentType { get; }
}
