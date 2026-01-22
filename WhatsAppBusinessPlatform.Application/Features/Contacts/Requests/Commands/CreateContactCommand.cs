using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Common;
using WhatsAppBusinessPlatform.Application.DTOs.Contacts;

namespace WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Commands;
public record CreateContactCommand(CreateContactRequest ContactRequest, string IdempotencyKey) 
    : ICommand<Result<string>>, IIdempotentCommand;
