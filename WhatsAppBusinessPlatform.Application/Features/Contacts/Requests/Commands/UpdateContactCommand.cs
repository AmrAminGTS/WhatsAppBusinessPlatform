using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Contacts;

namespace WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Commands;

public record UpdateContactCommand(string ContactId, UpdateContactRequest Request) : ICommand<Result>;
