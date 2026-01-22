using MediatAmR.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Commands;

namespace WhatsAppBusinessPlatform.API.Web.Controllers;
public sealed class WebhookController : BaseController
{
    private readonly IConfiguration _config;
    private readonly IRequestSender _sender;
    private readonly ILogger<WebhookController> _logger;
    public WebhookController(
        IConfiguration config,
        IRequestSender sender,
        ILogger<WebhookController> logger)
    {
        _config = config;
        _sender = sender;
        _logger = logger;
    }

    // Example: GET /whatsapp?hub.mode=subscribe&hub.verify_token=token&hub.challenge=CHALLENGE
    [HttpGet("whatsapp")]
    public ActionResult VerifyWebhook()
    {
        IQueryCollection query = Request.Query;
        string mode = query["hub.mode"].ToString();
        string token = query["hub.verify_token"].ToString();
        string challenge = query["hub.challenge"].ToString();

        string expectedToken = _config["TOKEN"] ?? "token";
        _logger.LogCritical("Verification received!!");
        if (mode == "subscribe" && token == expectedToken)
        {
            return Ok(challenge); // respond with challenge plain text
        }

        return BadRequest();
    }

    // POST /whatsapp
    [HttpPost("whatsapp")]
    public async Task<ActionResult> ReceiveUpdatess(CancellationToken cancellationToken = default)
    {
        Request.EnableBuffering();
        Result result = await _sender.Send(new ReceiveUpdateCommand(Request), cancellationToken);
        return result.IsSuccess ? Ok() : ToProblemDetails(result.Error);
    }
}
