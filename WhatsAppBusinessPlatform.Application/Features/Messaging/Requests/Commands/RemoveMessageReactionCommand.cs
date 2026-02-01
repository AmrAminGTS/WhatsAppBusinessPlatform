using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;

namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Commands;

public sealed record RemoveMessageReactionCommand(string ReactionId) : ICommand<Result<ChatLastUpdateResponse?>>;
