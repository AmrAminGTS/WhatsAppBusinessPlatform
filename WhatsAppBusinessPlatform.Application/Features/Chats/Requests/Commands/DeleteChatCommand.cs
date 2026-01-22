using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Common;

namespace WhatsAppBusinessPlatform.Application.Features.Chats.Requests.Commands;

internal sealed record DeleteChatCommand(string PhoneNumberId) : ICommand<Result>;
