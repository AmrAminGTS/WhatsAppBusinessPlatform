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

    public async Task<Result<WAMessage>> SaveReactionAsync(
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

        MessageReaction? existingReaction = direction == MessageDirection.Received
            ? await reactionSet.FirstOrDefaultAsync(r
                => r.ReactedToMessageId == reactedToMessageId
                && r.Direction == direction
                && r.ReactedByAccountId == sentByAccount.Id,
                cancellationToken)
            : await reactionSet.FirstOrDefaultAsync(r
                => r.ReactedToMessageId == reactedToMessageId
                && r.Direction == direction,
                cancellationToken);

        bool emojiFound = string.IsNullOrWhiteSpace(emoji) == false;

        // if reaction exists and emoji is missing, remove reaction
        if (existingReaction is not null && emojiFound == false)
        {
            // remove reaction if emoji is missing
            reactionSet.Remove(existingReaction);
        }
        // if reaction exists and emoji is present, update reaction
        else if (existingReaction is not null && emojiFound)
        {
            existingReaction.Emoji = emoji!;
            reactionSet.Update(existingReaction);
        }
        // if reaction does not exist and emoji is present, create reaction
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
        // if reaction does not exist and emoji is missing, do nothing.
        // this case only occurs when trying to remove a non-existing reaction.
        return Result.Success(reactedToMessage);
    }
}
