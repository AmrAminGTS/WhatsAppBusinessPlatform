using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Authentication;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Commands;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;

namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Handlers.Commands;

internal sealed class MarkMessagesAsReadCommandHandler(
    IUserContext userContext,
    IIdempotencyHandler idempotencyHandler,
    IUnitOfWork unitOfWork) : ICommandHandler<MarkMessagesAsReadCommand, Result>
{
    public async Task<Result> Handle(MarkMessagesAsReadCommand request, CancellationToken cancellationToken = default)
    {
        string userId = userContext.UserId;

        return await idempotencyHandler.HandleWithIdempotencyAsync(request.IdempotencyKey, async () => {

            WAMessage? lastReceivedMessage = await unitOfWork.Repository<WAMessage>()
                .Where(m => m.Id == request.LastReceivedMessageId)
                .FirstOrDefaultAsync();

            if (lastReceivedMessage is null)
            {
                return MessagingErrors.MessageNotFound(request.LastReceivedMessageId);
            }

            List<MessageReader> entries = await unitOfWork.Repository<WAMessage>()
                .Where(m => m.ContactPhoneNumber == request.ContactPhoneNumber
                    && m.Direction == MessageDirection.Received
                    && m.MessageReaders.Any(mr => mr.UserId == userId) == false
                    && m.DateTimeOffset <= lastReceivedMessage.DateTimeOffset)
                .AsNoTracking()
                .Select(m => new MessageReader { MessageId = m.Id, UserId = userId })
                .ToListAsync();

            unitOfWork.Repository<MessageReader>().AddRange(entries);
            Result<int> saveResult = await unitOfWork.SaveAsync(cancellationToken);
            if (saveResult.IsFailure)
            {
                return saveResult.Error;
            }
            return saveResult;

        }, cancellationToken: cancellationToken);
    }
}
