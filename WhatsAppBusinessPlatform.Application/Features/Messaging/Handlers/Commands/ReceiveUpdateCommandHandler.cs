using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LinqKit;
using MediatAmR.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Persistence;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;
using WhatsAppBusinessPlatform.Application.DTOs.Messaging.MessageContentTypes;
using WhatsAppBusinessPlatform.Application.DTOs.Webhook;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Commands;
using WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Events;
using WhatsAppBusinessPlatform.Application.Mapping;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Chats;
using WhatsAppBusinessPlatform.Application.Mapping.Maps.Webhook;
using WhatsAppBusinessPlatform.Domain.Common;
using WhatsAppBusinessPlatform.Domain.Entities.Messages;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses;
using WhatsAppBusinessPlatform.Domain.Entities.MessageStatuses.Events;
using WhatsAppBusinessPlatform.Domain.Entities.WAAccounts;

namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Handlers.Commands;
internal sealed class ReceiveUpdateCommandHandler : ICommandHandler<ReceiveUpdateCommand, Result>
{
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReceiveUpdateCommandHandler> _logger ;
    private readonly IConfiguration _configuration;
    private readonly IIdempotencyHandler _idempotencyHandler;
    private const int _receivedUpdateTTLMinutes = 10080;
    public ReceiveUpdateCommandHandler(
        IConfiguration configuration,
        ILogger<ReceiveUpdateCommandHandler> logger,
        JsonSerializerOptions serializerOptions,
        IUnitOfWork unitOfWork,
        IIdempotencyHandler idempotencyHandler)
    {
        _configuration = configuration;
        _logger = logger;
        _serializerOptions = serializerOptions;
        _unitOfWork = unitOfWork;
        _idempotencyHandler = idempotencyHandler;
    }

    public async Task<Result> Handle(ReceiveUpdateCommand request, CancellationToken cancellationToken = default)
    {
        HttpRequest httpRequest = request.ReceiveMessageRequest;
        string? xHubSignature = httpRequest.Headers["X-Hub-Signature-256"];
        using var reader = new StreamReader(httpRequest.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        string body = await reader.ReadToEndAsync(cancellationToken);
        httpRequest.Body.Position = 0; // reset for further middle-ware if needed

        // Signature verification (X-Hub-Signature) using HMAC SHA1 like express-x-hub
        if (VerifyXHubSignature(body, xHubSignature) == false)
        {
            return MessagingErrors.UnauthorizedXHubSignature();
        }

        // Store the received update
        ReceivedUpdate? receivedUpdate = JsonSerializer.Deserialize<ReceivedUpdate>(body);
        if (receivedUpdate is null)
        {
            // Log Error
            _logger.LogError("Received Update is null");
            return Result.Success();
        }

        // Determine the type of Update (Message or Status change)
        JsonElement rawJson = JsonSerializer.Deserialize<JsonElement>(body);
        JsonElement convertedJson = JsonSerializer.SerializeToElement(receivedUpdate, _serializerOptions);
        byte[] rawBytes = Encoding.UTF8.GetBytes(body);

        WebhookMetadata? metadata = receivedUpdate.Entry.First().Changes.First().Value.Metadata;
        WebhookReceivedMessage? message = receivedUpdate.Entry.First().Changes.First().Value.Messages?.FirstOrDefault();
        WebhookReceivedMessageStatus? status = receivedUpdate.Entry.First().Changes.First().Value.Statuses?.FirstOrDefault();
        if (metadata == null)
        {
            _logger.LogError("Received Update Metadata is null");
            return Result.Success();
        }

        if (JsonElement.DeepEquals(rawJson, convertedJson) == false)
        {
            _logger.LogError("Raw Update request doesn't match the converted one");
        }
        Result saveResult;
        if (message is not null)
        {
            // Handle Idempotency ( key = messageId )
            string idempotencyKey = message.MessageId;
            saveResult = await _idempotencyHandler.HandleWithIdempotencyAsync(
                idempotencyKey,
                () => SaveMessageAsync(message, metadata.PhoneNumberId, rawBytes, cancellationToken),
                _receivedUpdateTTLMinutes,
                cancellationToken: cancellationToken);
        }
        else if (status is not null)
        {
            // Handle Idempotency ( key = messageId + status)
            string idempotencyKey = string.Join("__", status.MessageId, status.Status);
            saveResult = await _idempotencyHandler.HandleWithIdempotencyAsync(
                idempotencyKey,
                () => SaveStatusAsync(status, rawBytes, cancellationToken),
                _receivedUpdateTTLMinutes,
                cancellationToken: cancellationToken);
        }
        else
        {
            // Log Error
            _logger.LogCritical("Received Update is Unknown");
            return Result.Success();
        }
        if (saveResult.IsFailure)
        {
            return saveResult.Error;
        }
        return Result.Success();
    }

    // Private Methods
    private async Task<Result<int>> SaveMessageAsync(
        WebhookReceivedMessage receivedMessage,
        string phoneNumberId,
        byte[] rawMessageBytes,
        CancellationToken cancellationToken)
    {
        // Handle receiving message from Anonymous
        WAAccount sentByAccount = await _unitOfWork.Repository<WAAccount>()
            .GetOrAddAsync(a => a.PhoneNumber == receivedMessage.FromPhoneNumber,
                new() { PhoneNumber = receivedMessage.FromPhoneNumber },
                cancellationToken);

        IRepository<WAMessage> messageRepository = _unitOfWork.Repository<WAMessage>();
        WAMessage messageEntry = receivedMessage.MapToWAMessage(sentByAccount, phoneNumberId, rawMessageBytes);

        if (MessageContentType.FromString(receivedMessage.MessageType) == MessageContentType.Reaction)
        {
            ReactionMessageContent? reactionContent = JsonSerializer.Deserialize<ReactionMessageContent>(receivedMessage.JsonContent);
            ArgumentNullException.ThrowIfNull(reactionContent);
            var when = DateTimeOffset.FromTimestampString(receivedMessage.Timestamp);

            if (!string.IsNullOrWhiteSpace(reactionContent.Emoji))
            {
                Result<Guid> saveReactionResult = await _unitOfWork.ReactionRepository.AddOrUpdateReactionAsync(
                    messageEntry,
                    reactionContent.ReactedToMessageId,
                    reactionContent.Emoji!,
                    sentByAccount.Id,
                    null,
                    when,
                    MessageDirection.Received,
                    cancellationToken);

                if (saveReactionResult.IsFailure)
                {
                    return saveReactionResult.Error;
                }
            }
            else
            {
                MessageReaction? existingReaction = await _unitOfWork.ReactionRepository
                    .FirstOrDefaultAsync(r
                        => r.Direction == MessageDirection.Received
                        && r.ReactedToMessageId == reactionContent.ReactedToMessageId
                        && r.ContactAccountId == sentByAccount.Id
                        , cancellationToken);

                if(existingReaction == null)
                {
                    return GeneralErrors.NotFound(nameof(MessageReaction));
                }

                // persist the emoji message with no emoji
                messageRepository.Add(messageEntry);
                _unitOfWork.ReactionRepository.Delete(existingReaction);
            }
            _logger.LogInformation("Received Update is Reaction Message");
            WAMessage? reactedToMessage = await _unitOfWork.Repository<WAMessage>()
                .Include(m => m.Reactions)
                .FirstOrDefaultAsync(m => m.Id == reactionContent.ReactedToMessageId, cancellationToken);

            if (reactedToMessage == null)
            {
                return MessagingErrors.MessageNotFound(reactionContent.ReactedToMessageId);
            }

            messageEntry.Raise(
                new MessageReactionsChangedDomainEvent(
                    messageEntry.ContactPhoneNumber,
                    reactedToMessage.Id,
                    reactionContent.Emoji,
                    when,
                    JsonSerializer.Deserialize<object>(reactedToMessage.JsonContent)!,
                    reactedToMessage.GetReactionsSummary
                )
            );
        }
        else
        {
            messageRepository.Add(messageEntry);
            messageEntry.Raise(new MessageReceivedDomainEvent(messageEntry.Id));
            _logger.LogInformation("Received Update is Message");
        }
        return await _unitOfWork.SaveAsync(cancellationToken);
    }

    private async Task<Result<int>> SaveStatusAsync(
        WebhookReceivedMessageStatus status,
        byte[] rawBytes,
        CancellationToken cancellationToken)
    {
        WAMessage? existingMessage = await _unitOfWork.Repository<WAMessage>()
            .Where(m => m.Id == status.MessageId)
            .SingleOrDefaultAsync(cancellationToken);

        if (existingMessage == null)
        {
            _logger.LogWarning("Received Status for non-existing Message with Id: {MessageId}", status.MessageId);
            return MessagingErrors.MessageNotFound(status.MessageId);
        }

        MessageStatus statusEntry = status.MapToWAMessageStatus(existingMessage, rawBytes);

        _unitOfWork.Repository<MessageStatus>().Add(statusEntry);
        statusEntry.Raise(new MessageStatusReceivedDomainEvent(status.RecipientId, status.MessageId, statusEntry.Status, statusEntry.Error?.Details));
        _logger.LogInformation("Received Update is Status");

        return await _unitOfWork.SaveAsync(cancellationToken);
    }

    private bool VerifyXHubSignature(string payload, string? signatureHeader)
    {
        // Verifies signature header of the form "sha1=<hex>"
        if (string.IsNullOrWhiteSpace(signatureHeader))
        {
            return false;
        }

        // secret stored in environment: APP_SECRET
        string? secret = _configuration["APP_SECRET"];
        if (string.IsNullOrEmpty(secret))
        {
            return false;
        }

        // expected format: "sha256=hex"
        const string prefix = "sha256=";
        if (!signatureHeader.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        string signatureHex = signatureHeader.Substring(prefix.Length);

        // compute HMAC-SHA256
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        string computedHex = Convert.ToHexString(hash).ToLowerInvariant();

        return CryptographicEquals(signatureHex, computedHex);
    }

    // constant-time comparison
    private static bool CryptographicEquals(string a, string b)
    {
        if (a == null || b == null)
        {
            return false;
        }

        byte[] aBytes = Encoding.UTF8.GetBytes(a);
        byte[] bBytes = Encoding.UTF8.GetBytes(b);

        bool equal = CryptographicOperations.FixedTimeEquals(aBytes, bBytes);

        // zero out the byte arrays if they contain sensitive data
        Array.Clear(aBytes, 0, aBytes.Length);
        Array.Clear(bBytes, 0, bBytes.Length);

        return equal;
    }
}
