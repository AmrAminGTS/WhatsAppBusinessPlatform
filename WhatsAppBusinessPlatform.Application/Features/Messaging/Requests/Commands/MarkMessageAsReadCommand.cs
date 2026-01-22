using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Common;

namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Commands;

public sealed record MarkMessagesAsReadCommand(string ContactPhoneNumber, string LastReceivedMessageId, string IdempotencyKey)
    : ICommand<Result>, IIdempotentCommand;
