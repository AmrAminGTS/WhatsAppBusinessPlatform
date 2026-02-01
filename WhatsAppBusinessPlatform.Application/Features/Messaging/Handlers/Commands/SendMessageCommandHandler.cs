using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Authentication;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Abstractions.ThirdParties;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Commands;
using WhatsAppBusinessPlatform.Application.Mapping;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Webhook;
using WhatsAppBusinessPlatform.Domain.Common;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Handlers.Commands;

public sealed class SendMessageCommandHandler<TMessageContent> : ICommandHandler<SendMessageCommand<TMessageContent>, Result<string>>
    where TMessageContent : IMessageContentType
{
    private readonly IWAHttpClient _waHttpClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserContext _userContext;
    private readonly IIdempotencyHandler _idempotencyHandler;
    public SendMessageCommandHandler(
        IWAHttpClient waHttpClient,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IUserContext userContext,
        IIdempotencyHandler idempotencyHandler)
    {
        _waHttpClient = waHttpClient;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _userContext = userContext;
        _idempotencyHandler = idempotencyHandler;
    }
    public async Task<Result<string>> Handle(SendMessageCommand<TMessageContent> request, CancellationToken cancellationToken = default)
        => await _idempotencyHandler.HandleWithIdempotencyAsync<string>(request.IdempotencyKey, async () =>
        {
            // Validate ContactAccount.
            string contactPhone = request.SendWAMessageRequest.RecipientPhoneNumber;

            WAAccount? sendToAccount = await _unitOfWork.Repository<WAAccount>()
                .Where(a => a.PhoneNumber == contactPhone)
                .SingleOrDefaultAsync(cancellationToken);
            if (sendToAccount == null)
            {
                return GeneralErrors.NotFound(nameof(WAAccount), contactPhone);
            }

            // Send message Request to WhatsApp Business Platform API
            Result<string> sendResult = await _waHttpClient.SendMessageAsync(request.SendWAMessageRequest, cancellationToken);
            if (sendResult.IsFailure)
            {
                return sendResult.Error;
            }

            WAMessage message = request.SendWAMessageRequest
                .MapToWAMessage(sendResult.Value, sendToAccount, _userContext.UserId, _dateTimeProvider.UtcNow);
            _unitOfWork.Repository<WAMessage>().Add(message);

            Result saveResult = await _unitOfWork.SaveAsync(cancellationToken);
            if (saveResult.IsFailure)
            {
                return saveResult.Error;
            }
            return sendResult.Value;

        }, cancellationToken: cancellationToken);
}
