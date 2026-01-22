namespace WhatsAppBusinessPlatform.Application.Abstractions.Shared;

public interface IListWithPaginationInfo<T>
{
    ICollection<T> Rows { get; set; } 
    IResponsePaginationInfo PaginationInfo { get; set; }
}
