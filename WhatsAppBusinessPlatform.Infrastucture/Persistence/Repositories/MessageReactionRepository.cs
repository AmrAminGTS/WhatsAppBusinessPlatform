using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Infrastucture.Persistence.Repositories;

public class MessageReactionRepository(ApplicationDbContext context) 
    : Repository<MessageReaction>(context), IMessageReactionRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<Guid>> AddOrUpdateReactionAsync(
        WAMessage message,
        string reactedToMessageId, 
        string emoji,
        string? contactAccountId,
        string? sentByUserId,
        DateTimeOffset reactionTime,
        MessageDirection direction,
        CancellationToken cancellationToken)
    {
        DbSet<MessageReaction> reactionSet = _context.Set<MessageReaction>();

        MessageReaction? existingReaction = await reactionSet
            .FirstOrDefaultAsync(r 
                => r.Direction == direction 
                && r.ReactedToMessageId == reactedToMessageId
                && r.ContactAccountId == contactAccountId 
                && r.UserId == sentByUserId, cancellationToken);

        WAMessage? reactedToMessage = await _context.Set<WAMessage>()
            .FirstOrDefaultAsync(m => m.Id == reactedToMessageId, cancellationToken);

        if (reactedToMessage == null)
        {
            return MessagingErrors.MessageNotFound(reactedToMessageId);
        }

        // if reaction exists, update reaction
        if (existingReaction != null)
        {
            existingReaction.Emoji = emoji!;
            existingReaction.DateTimeOffset = reactionTime;
            existingReaction.ContactAccountId = contactAccountId;
            existingReaction.Message = message;
            existingReaction.MessageId = message.Id;

            reactionSet.Update(existingReaction);
            return existingReaction.Id;
        }
        // if reaction does not exist, create reaction
        else 
        {
            MessageReaction reaction = new()
            {
                Emoji = emoji!,
                DateTimeOffset = reactionTime,
                Direction = direction,
                ReactedToMessage = reactedToMessage,
                ReactedToMessageId = reactedToMessage.Id,
                MessageId = message.Id,
                Message = message,
                ContactAccountId = contactAccountId,
                UserId = sentByUserId
            };
            reactionSet.Add(reaction);
            return reaction.Id;
        }
    }
}
