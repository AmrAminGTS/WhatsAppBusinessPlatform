using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Application.Abstractions.Persistence;

public interface IMessageReactionRepository: IRepository<MessageReaction>
{
    Task<Result<Guid>> AddOrUpdateReactionAsync(WAMessage message, string reactedToMessageId, string emoji, string? contactAccountId, string? sentByUserId, DateTimeOffset reactionTime, MessageDirection direction, CancellationToken cancellationToken);
}
