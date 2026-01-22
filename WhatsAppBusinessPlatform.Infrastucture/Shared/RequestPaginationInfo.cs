using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;

namespace WhatsAppBusinessPlatform.Infrastucture.Shared;

public sealed class RequestPaginationInfo : IRequestPaginationInfo
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
