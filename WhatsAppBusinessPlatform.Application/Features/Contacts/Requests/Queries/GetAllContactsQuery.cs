using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Application.Abstractions;
using WhatsAppBusinessPlatform.Application.Abstractions.Messaging;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.DTOs.Contacts;

namespace WhatsAppBusinessPlatform.Application.Features.Contacts.Requests.Queries;

public sealed record GetAllContactsQuery(IRequestPaginationInfo PaginationInfo) 
    : IQuery<IListWithPaginationInfo<GetAllContactsResponse>>;
