using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Common;

namespace WhatsAppBusinessPlatform.Application.Features.Messaging.Requests.Commands;
public sealed record ReceiveUpdateCommand(HttpRequest ReceiveMessageRequest) : ICommand<Result>;
