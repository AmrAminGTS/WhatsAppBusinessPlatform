using System.Globalization;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Authentication;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Application.Features.Chats.Requests.Queries;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Chats;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Application.Features.Chats.Handlers.Queries;
internal sealed class GetAllChatsQueryHandler(
    IUnitOfWork unitOfWork,
    IUserContext userContext) : 
    IQueryHandler<GetAllChatsQuery, GetAllChatsResponse>
{
    public async Task<GetAllChatsResponse> Handle(GetAllChatsQuery request, CancellationToken cancellationToken = default) 
    {
        string userId = userContext.UserId;
        IListWithPaginationInfo<ChatListItemDto> result = await unitOfWork.Repository<WAAccount>()
            .Include(a => a.Messages).ThenInclude(m => m.Reactions).ThenInclude(mr => mr.ContactAccount)
            .AsNoTracking()
            .AsExpandable()
            .OrderByDescending(c => c.Messages
                .Where(m => m.ContentType != MessageContentType.Reaction)
                .OrderByDescending(m => m.DateTimeOffset)
                .Select(m => m.DateTimeOffset)
                .FirstOrDefault()
             )
            .ThenBy(c => c.PhoneNumber) // fallback if no messages
            .Select(WAAccount.ProjectToChatListItemDto(userId))
            .ToListWithPaginationInfoAsync(request.PaginationInfo, cancellationToken);

        GetAllChatsResponse response = new() { Chats = result };
        return response;
    }
}
