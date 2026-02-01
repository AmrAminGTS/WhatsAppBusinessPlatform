using MediatAmR.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Commands;

namespace WhatsAppBusinessPlatform.API.Web.Controllers;
public sealed class MessagingController(IRequestSender sender) : BaseController
{
    [HttpPost("SendTextMessage")]
    public async Task<ActionResult> SendTextMessageAsync(SendWAMessageRequest<TextMessageContent> request, [FromHeader]string idempotencyKey)
    {
        Result<string> result = await sender.Send(new SendMessageCommand<TextMessageContent>(request, idempotencyKey));
        return result.IsSuccess ? Ok(result.Value) : ToProblemDetails(result.Error);
    }

    [HttpPut("ReactToMessage")]
    public async Task<ActionResult> ReactToMessageAsync(ReactToMessageRequest request)
    {
        Result<string> result = await sender.Send(new ReactToMessageCommand(request));
        return result.IsSuccess ? Ok(result.Value) : ToProblemDetails(result.Error);
    }

    [HttpDelete("RemoveMessageReaction")]
    public async Task<ActionResult<ChatLastUpdateResponse>> RemoveMessageReactionAsync(string id)
    {
        Result<ChatLastUpdateResponse?> result = await sender.Send(new RemoveMessageReactionCommand(id));
        return result.IsSuccess ? Ok(result.Value) : ToProblemDetails(result.Error);
    }

    [HttpPost("MarkMessagesAsRead")]
    public async Task<ActionResult> MarkMessagesAsReadAsync([FromHeader]string idempotencyKey, string contactPhoneNumber, string lastReceivedMessageId)
    {
        Result result = await sender.Send(new MarkMessagesAsReadCommand(contactPhoneNumber, lastReceivedMessageId, idempotencyKey));
        return result.IsSuccess ? Ok() : ToProblemDetails(result.Error);
    }

    // Send Template Message
    // Send File (may be deferred)
}
