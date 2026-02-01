using LinqKit;
using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Abstractions.ThirdParties;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Commands;
using WhatsAppBusinessPlatform.Application.Mapping;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Chats;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Webhook;
using WhatsAppBusinessPlatform.Domain.Common;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;
namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Handlers.Commands;

public sealed class RemoveMessageReactionCommandHandler 
    : ICommandHandler<RemoveMessageReactionCommand, Result<ChatLastUpdateResponse?>>
{
    private readonly IWAHttpClient _waHttpClient;
    private readonly IUnitOfWork _unitOfWork;
    public RemoveMessageReactionCommandHandler(
        IWAHttpClient waHttpClient,
        IUnitOfWork unitOfWork)
    {
        _waHttpClient = waHttpClient;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<ChatLastUpdateResponse?>> Handle(RemoveMessageReactionCommand request, CancellationToken cancellationToken = default)
    {
        MessageReaction? existingReaction = await _unitOfWork.ReactionRepository
            .Include(r => r.Message)
            .FirstOrDefaultAsync(r => r.Id == Guid.Parse(request.ReactionId), cancellationToken);

        if (existingReaction == null)
        {
            return GeneralErrors.NotFound(nameof(MessageReaction), request.ReactionId);
        }

        // Send message Request to WhatsApp Business Platform API
        var messageObject = new SendWAMessageRequest<ReactionMessageContent>()
        {
            RecipientPhoneNumber = existingReaction.Message!.ContactPhoneNumber,
            MessageContent = new ReactionMessageContent
            {
                ReactedToMessageId = existingReaction.ReactedToMessageId,
                Emoji = "" // Empty emoji to remove reaction
            },
        };
        Result<string> sendResult = await _waHttpClient.SendMessageAsync(messageObject, cancellationToken);
        if (sendResult.IsFailure)
        {
            return sendResult.Error;
        }

        _unitOfWork.ReactionRepository.Delete(existingReaction);

        Result saveResult = await _unitOfWork.SaveAsync(cancellationToken);
        if (saveResult.IsFailure)
        {
            return saveResult.Error;
        }

        // Getting new Last Update of Chat
        ChatLastUpdateResponse? lastUpdate = await _unitOfWork.AccountRepository
            .Select(a => WAAccount.GetLastUpdateExpr().Invoke(a))
            .FirstOrDefaultAsync(cancellationToken);

        return lastUpdate;
    }
}
