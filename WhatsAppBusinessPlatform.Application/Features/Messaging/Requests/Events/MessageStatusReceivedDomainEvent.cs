using System;
using System.Collections.Generic;
using System.Text;
using MediatAmR.Abstractions;
using WhatsAppBusinessPlatform.Domain.Common;

namespace WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses.Events;

public sealed record MessageStatusReceivedDomainEvent(
    string ContactPhoneNumber,
    string MessageId,
    MessageStatusType Status,
    string? Error = null) 
    : IEvent;
