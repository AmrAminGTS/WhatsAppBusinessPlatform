using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Common;

namespace WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Queries;

public record CheckIfPhoneIsOnWhatsAppQuery(string PhoneNumber) : IQuery<Result>;
