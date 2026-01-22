using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;

namespace WhatsAppBusinessPlatform.Application.Common;
public sealed class ResponsePaginationInfo : IResponsePaginationInfo
{
    public long TotalRowsCount { get; set; }
    public long TotalPagesCount { get; set; }
    public long CurrentPageIndex { get; set; }
}
