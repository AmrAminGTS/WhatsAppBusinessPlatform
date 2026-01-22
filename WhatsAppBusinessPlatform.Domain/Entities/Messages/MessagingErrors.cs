using WhatsAppBusinessPlatform.Domain.Common;

namespace WhatsAppBusinessPlatform.Domain.Entities.Messages;

public static class MessagingErrors
{
    public static DomainError UnsuccessfulStatusCode(string error) => DomainError.Problem("Messaging.UnsuccessfulStatusCode", error);
    public static DomainError NotSent(string error) => DomainError.Failure("Messaging.NotSent", error);
    public static DomainError UnauthorizedXHubSignature() => DomainError.Unauthorized("Messaging.UnauthorizedXHubSignature");
    public static DomainError UnknownWebHookRequest() => DomainError.Problem("Messaging.UnknownWebHookRequest");
    public static DomainError MessageNotFound(string messageId) => DomainError.NotFound("Messaging.MessageNotFound", messageId);
}
