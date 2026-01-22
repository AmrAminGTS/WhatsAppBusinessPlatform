using System.Threading.Tasks;
using MediatAmR.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Application.Features.Chats.Requests.Queries;
using WhatsAppBusinessPlatform.Infrastucture.Shared;

namespace WhatsAppBusinessPlatform.API.Web.Controllers;

public class ChatController(IRequestSender mediator) : BaseController
{
    [HttpGet("AllChats")]
    public async Task<ActionResult<GetAllChatsResponse>> GetAllChats([FromQuery] RequestPaginationInfo paginationInfo)
    {
        GetAllChatsResponse result = await mediator.Send(new GetAllChatsQuery(paginationInfo));
        return Ok(result);
    }

    [HttpGet("{phoneNumber}")]
    public async Task<ActionResult<GetChatResponse>> GetChat(string phoneNumber, [FromQuery] RequestPaginationInfo paginationInfo)
    {
        GetChatResponse result = await mediator.Send(new GetChatQuery(phoneNumber, paginationInfo));
        return Ok(result);
    }

    [HttpDelete("Delete/{phoneNumber}")]
    public ActionResult DeleteChat(string phoneNumber) => throw new NotImplementedException(phoneNumber);
}
