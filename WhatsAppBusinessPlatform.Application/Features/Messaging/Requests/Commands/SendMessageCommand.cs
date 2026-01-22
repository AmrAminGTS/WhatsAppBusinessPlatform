using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;

namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Commands;
public sealed record SendMessageCommand<TMessageContent>(SendWAMessageRequest<TMessageContent> SendWAMessageRequest, string IdempotencyKey) 
    : ICommand<Result<string>>, IIdempotentCommand where TMessageContent : IMessageContentType;
