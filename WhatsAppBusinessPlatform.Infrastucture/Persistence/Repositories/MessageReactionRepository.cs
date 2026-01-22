using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Infrastucture.Persistence.Repositories;

public class MessageReactionRepository(ApplicationDbContext context) : Repository<MessageReaction>(context), IMessageReactionRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> SaveReactionAsync(
        string reactedToMessageId, 
        string? emoji,
        WAAccount sentByAccount,
        DateTimeOffset reactionTime,
        MessageDirection direction,
        CancellationToken cancellationToken)
    {
        WAMessage? reactedToMessage = await _context.Set<WAMessage>()
            .FirstOrDefaultAsync(m => m.Id == reactedToMessageId, cancellationToken);

        if (reactedToMessage == null)
        {
            return MessagingErrors.MessageNotFound(reactedToMessageId);
        }

        DbSet<MessageReaction> reactionSet = _context.Set<MessageReaction>();

        MessageReaction? existingReaction = await reactionSet.FirstOrDefaultAsync(r
            => r.ReactedToMessageId == reactedToMessageId
            && r.Direction == direction
            && r.ReactedByAccountId == sentByAccount.Id,
            cancellationToken);

        bool emojiFound = string.IsNullOrWhiteSpace(emoji) == false;

        if (existingReaction is not null && emojiFound == false)
        {
            // remove reaction if emoji is missing
            reactionSet.Remove(existingReaction);
        }
        else if (existingReaction is not null && emojiFound)
        {
            existingReaction.Emoji = emoji!;
            reactionSet.Update(existingReaction);
        }
        else if (existingReaction is null && emojiFound)
        {
            MessageReaction reaction = new()
            {
                Emoji = emoji!,
                DateTimeOffset = reactionTime,
                Direction = direction,
                ReactedToMessage = reactedToMessage,
                ReactedToMessageId = reactedToMessageId,
                ReactedByAccountId = sentByAccount.Id,
                ReactedByAccount = sentByAccount
            };
            reactionSet.Add(reaction);
        }
        return Result.Success();
    }
}
