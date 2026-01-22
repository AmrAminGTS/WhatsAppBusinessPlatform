namespace WhatsAppBusinessPlatform.Application.Abstractions.Shared;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
