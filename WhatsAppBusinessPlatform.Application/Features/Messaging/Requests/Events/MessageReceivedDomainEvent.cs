using System;
using System.Collections.Generic;
using System.Text;
using MediatAmR.Abstractions;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Events;

public sealed record MessageReceivedDomainEvent(WAMessage Message) : IEvent;
public sealed record MessageReactionsChangedDomainEvent(
    string ContactPhone,
    string MessageId,
    string? Emoji,
    DateTimeOffset When,
    object ReactedToMessageContent,
    IDictionary<string, int> ReactionSummary) : IEvent;
