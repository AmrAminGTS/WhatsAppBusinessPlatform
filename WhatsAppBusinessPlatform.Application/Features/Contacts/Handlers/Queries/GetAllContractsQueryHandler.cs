using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Authentication;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Contacts;
using WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Queries;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Contacts;
using WhatsAppBusinessPlatform.Domain.Entities.Contacts;

namespace WhatsAppBusinessPlatform.Application.Features.Contacts.Handlers.Queries;

internal sealed class GetAllContractsQueryHandler : IQueryHandler<GetAllContactsQuery, IListWithPaginationInfo<GetAllContactsResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    public GetAllContractsQueryHandler(
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<IListWithPaginationInfo<GetAllContactsResponse>> Handle(
        GetAllContactsQuery request,
        CancellationToken cancellationToken = default)
    {
        IListWithPaginationInfo<GetAllContactsResponse> result = await _unitOfWork.Repository<Contact>()
            .Where(c => c.CreatedBy == _userContext.UserId && c.FirstName != null)
            .AsNoTracking()
            .OrderBy(c => c.FirstName)
            .ThenBy(c => c.LastName)
            .Select(Contact.ProjectToGetAllContactsResponse())
            .ToListWithPaginationInfoAsync(request.PaginationInfo, cancellationToken);
        return result;
    }
}
