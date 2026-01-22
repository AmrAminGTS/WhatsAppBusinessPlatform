using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Common;

namespace WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Commands;

public record DeleteContactCommand(string ContactId) : ICommand<Result>;
