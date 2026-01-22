using WhatsAppBusinessPlatform.Application.Abstractions.Shared;

namespace WhatsAppBusinessPlatform.Infrastucture.Shared;
internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
