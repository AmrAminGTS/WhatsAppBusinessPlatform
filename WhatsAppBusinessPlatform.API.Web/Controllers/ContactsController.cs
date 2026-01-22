using System.Threading.Tasks;
using Azure.Core;
using MediatAmR.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Contacts;
using WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Commands;
using WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Queries;
using WhatsAppBusinessPlatform.Infrastucture.Shared;

namespace WhatsAppBusinessPlatform.API.Web.Controllers;
public sealed class ContactsController : BaseController
{
    private readonly IRequestSender _mediatAmr;
    public ContactsController(IRequestSender mediatAmr) => _mediatAmr = mediatAmr;

    [HttpGet("All")]
    public async Task<ActionResult<IListWithPaginationInfo<GetAllContactsResponse>>> GetAllContacts(
        [FromQuery] RequestPaginationInfo paginationInfo)
    {
        var query = new GetAllContactsQuery(paginationInfo);
        IListWithPaginationInfo<GetAllContactsResponse> result = await _mediatAmr.Send(query);
        return Ok(result);
    }
    
    [HttpGet("{contactId}")]
    public async Task<ActionResult> GetContactById(string contactId)
    {
        Result<GetContactByIdResponse> result = await _mediatAmr.Send(new GetContactByIdQuery(contactId));
        return result.IsSuccess ? Ok(result.Value) : ToProblemDetails(result.Error);
    }

    [HttpGet("/phone{phoneNumber}")]
    public async Task<ActionResult> GetContactByPhoneNumber(string phoneNumber)
    {
        Result<GetContactByIdResponse> result = await _mediatAmr.Send(new GetContactByPhoneNumberQuery(phoneNumber));
        return result.IsSuccess ? Ok(result.Value) : ToProblemDetails(result.Error);
    }

    [HttpPost("New")]
    public async Task<ActionResult> AddContact(CreateContactRequest request, [FromHeader] string idempotencyKey)
    {
        Result<string> result = await _mediatAmr.Send(new CreateContactCommand(request, idempotencyKey));
        return result.IsSuccess 
            ? CreatedAtAction(nameof(GetContactById), new { contactId = result.Value }, result.Value) 
            : ToProblemDetails(result.Error);
    }
        
    [HttpPatch("Edit/{contactId}")]
    public async Task<ActionResult> UpdateContact(string contactId, [FromBody] UpdateContactRequest request)
    {
        Result result = await _mediatAmr.Send(new UpdateContactCommand(contactId, request));
        return result.IsSuccess ? NoContent() : ToProblemDetails(result.Error);
    }

    [HttpDelete("Delete/{contactId}")]
    public async Task<ActionResult> DeleteContactAsync(string contactId)
    {
        Result result = await _mediatAmr.Send(new DeleteContactCommand(contactId));
        return result.IsSuccess ? NoContent() : ToProblemDetails(result.Error);
    }

    [HttpGet("CheckIfPhoneIsOnWhatsApp/{phoneNumber}")]
    public async Task<ActionResult<bool>> CheckIfPhoneIsOnWhatsAppAsync(string phoneNumber)
    {
        Result result = await _mediatAmr.Send(new CheckIfPhoneIsOnWhatsAppQuery(phoneNumber));
        return result.IsSuccess ? NoContent() : ToProblemDetails(result.Error);
    }
}
