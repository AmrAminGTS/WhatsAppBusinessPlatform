using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Application.Features.Chats.Requests.Queries;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Chats;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Application.Features.Chats.Handlers.Queries;
internal sealed class GetChatQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetChatQuery, GetChatResponse>
{
    public async Task<GetChatResponse> Handle(GetChatQuery request, CancellationToken cancellationToken = default)
    {
        IListWithPaginationInfo<ChatMessageResponse> messages = await unitOfWork.Repository<WAMessage>()
            .Include(m => m.Reactions)
            .Include(m => m.ReplyTo)
            .Where(m => m.ContactPhoneNumber == request.PhoneNumberId 
                && m.ContentType != MessageContentType.Reaction)
            .AsNoTracking()
            .OrderByDescending(m => m.DateTimeOffset)
            .Select(WAMessage.ProjectToChatMessageResponse())
            .ToListWithPaginationInfoAsync(request.PaginationInfo, cancellationToken);

        GetChatResponse response = new() { ChatMessages = messages };
        return response;
    }
}
