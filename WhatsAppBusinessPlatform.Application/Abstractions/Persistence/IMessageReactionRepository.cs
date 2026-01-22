using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Application.Abstractions.Persistence;

public interface IMessageReactionRepository
{
    Task<Result> SaveReactionAsync(string reactedToMessageId, string? emoji, WAAccount sentByAccount, DateTimeOffset reactionTime, MessageDirection direction, CancellationToken cancellationToken);
}
