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
public sealed class GetContactByIdQueryHandler : IQueryHandler<GetContactByIdQuery, Result<GetContactByIdResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    public GetContactByIdQueryHandler(
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<GetContactByIdResponse>> Handle(GetContactByIdQuery request, CancellationToken cancellationToken = default)
    {
        GetContactByIdResponse? result = await _unitOfWork.Repository<Contact>()
            .Where(c => c.Id == request.ContactId && c.CreatedBy == _userContext.UserId && c.FirstName != null)
            .AsNoTracking()
            .Select(Contact.ProjectToGetContactByIdResponse())
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            return ContactErrors.NotFound(request.ContactId);
        }
        GetContactByIdResponse mapped = result;
        return mapped;
    }
}
