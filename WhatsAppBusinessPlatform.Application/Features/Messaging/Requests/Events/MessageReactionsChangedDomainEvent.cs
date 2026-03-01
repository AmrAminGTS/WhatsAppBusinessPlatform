using MediatAmR.Abstractions;

namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Events;

public sealed record MessageReactionsChangedDomainEvent(
    string ContactPhone,
    string MessageId,
    string? Emoji,
    DateTimeOffset When,
    object ReactedToMessageContent,
    IDictionary<string, int> ReactionSummary) : IEvent;
