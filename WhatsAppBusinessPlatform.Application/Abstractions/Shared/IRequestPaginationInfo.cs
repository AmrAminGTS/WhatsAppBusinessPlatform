using System;
using System.Collections.Generic;
using System.Text;

namespace WhatsAppBusinessPlatform.Application.Abstractions.Shared;
public interface IRequestPaginationInfo
{
    int PageIndex { get; set; }
    int PageSize { get; set; }
}
