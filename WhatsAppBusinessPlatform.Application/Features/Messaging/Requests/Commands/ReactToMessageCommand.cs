using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging;

namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Commands;

public sealed record ReactToMessageCommand(ReactToMessageRequest SendWAMessageRequest)
    : ICommand<Result<string>>;
