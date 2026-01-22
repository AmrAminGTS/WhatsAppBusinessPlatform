using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Contacts;

namespace WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Queries;

public sealed record GetContactByIdQuery(string ContactId) : IQuery<Result<GetContactByIdResponse>>;
