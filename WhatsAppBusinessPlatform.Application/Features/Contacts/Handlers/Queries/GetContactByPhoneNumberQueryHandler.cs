using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Authentication;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Contacts;
using WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Queries;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Contacts;
using WhatsAppBusinessPlatform.Domain.Entities.Contacts;

namespace WhatsAppBusinessPlatform.Application.Features.Contacts.Handlers.Queries;
public sealed class GetContactByPhoneNumberQueryHandler : IQueryHandler<GetContactByPhoneNumberQuery, Result<GetContactByIdResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    public GetContactByPhoneNumberQueryHandler(
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<GetContactByIdResponse>> Handle(GetContactByPhoneNumberQuery request, CancellationToken cancellationToken = default)
    {
        GetContactByIdResponse? result = await _unitOfWork.Repository<Contact>()
            .Where(c => c.WAAccount.PhoneNumber == request.PhoneNumber 
                && c.CreatedBy == _userContext.UserId && c.FirstName != null)
            .AsNoTracking()
            .Select(Contact.ProjectToGetContactByIdResponse())
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            return ContactErrors.NotFound(request.PhoneNumber);
        }
        GetContactByIdResponse mapped = result;
        return mapped;
    }
}
