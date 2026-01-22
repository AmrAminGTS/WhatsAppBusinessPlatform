using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.DTOs.Chats;

namespace WhatsAppBusinessPlatform.Application.Features.Chats.Requests.Queries;

public sealed record GetChatQuery(string PhoneNumberId, IRequestPaginationInfo PaginationInfo) : IQuery<GetChatResponse>;
