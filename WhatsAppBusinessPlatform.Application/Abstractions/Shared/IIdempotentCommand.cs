namespace WhatsAppBusinessPlatform.Application.Abstractions.Shared;

public interface IIdempotentCommand
{
    string IdempotencyKey { get; }
}
