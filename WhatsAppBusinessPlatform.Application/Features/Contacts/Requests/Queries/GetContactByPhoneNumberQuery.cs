using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Contacts;

namespace WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Queries;

public sealed record GetContactByPhoneNumberQuery(string PhoneNumber) : IQuery<Result<GetContactByIdResponse>>;
