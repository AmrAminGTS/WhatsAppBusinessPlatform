namespace WhatsAppBusinessPlatform.Application.Abstractions.Shared;

public interface IResponsePaginationInfo
{
    long TotalRowsCount { get; set; }
    long TotalPagesCount { get; set; }
    long CurrentPageIndex { get; set; }
}
