using MediatAmR.Abstractions;
using Microsoft.AspNetCore.SignalR;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Commands;

namespace WhatsAppBusinessPlatform.API.Web.RealTime;

public sealed class MessagingHub(IRequestSender mediator, ILogger<MessagingHub> logger) : Hub<IMessagingHub>
{
    public async Task<string> SendMessage(SendWAMessageRequest<TextMessageContent> messageRequest, string idempotencyKey)
    {
        Result<string> result = await mediator.Send(new SendMessageCommand<TextMessageContent>(messageRequest, idempotencyKey));
        if (result.IsFailure)
        {
            logger.LogError("Error Code: {Code}", result.Error.Code);
        }
        return result.Value;
    }
}
