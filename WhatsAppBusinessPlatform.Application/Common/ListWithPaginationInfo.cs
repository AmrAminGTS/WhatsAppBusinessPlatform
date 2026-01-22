using WhatsAppBusinessPlatform.Application.Abstractions.Shared;

namespace WhatsAppBusinessPlatform.Application.Common;

public class ListWithPaginationInfo<T> : IListWithPaginationInfo<T>
{
    public ICollection<T> Rows { get; set; } = [];
    public IResponsePaginationInfo PaginationInfo { get; set; } = new ResponsePaginationInfo();
}
