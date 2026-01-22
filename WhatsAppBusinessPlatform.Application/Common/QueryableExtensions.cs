using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Application.Abstractions.Shared;
using WhatsAppBusinessPlatform.Application.Common;

namespace WhatsAppBusinessPlatform.Application.Common;

internal static class QueryableExtensions
{
    private const int DefaultMaxPageSize = 100;

    extension<T>(IQueryable<T> query)
    {
        public async Task<(IQueryable<T> PagedQuery, IResponsePaginationInfo PageInfo)> 
            ApplyPaginationAsync(IRequestPaginationInfo paginationInfo, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(paginationInfo);

            int totalRows = await query.CountAsync(cancellationToken);

            int pageIndex = Math.Max(1, paginationInfo.PageIndex); // assume 0-based
            int pageSize = (paginationInfo.PageSize <= 0 || paginationInfo.PageSize > DefaultMaxPageSize)
                ? DefaultMaxPageSize
                : paginationInfo.PageSize;

            long skipLong = (long)(pageIndex - 1) * pageSize;
            if (skipLong > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(paginationInfo), 
                    "Requested page results in skip larger than supported.");
            }

            int skip = (int)skipLong;

            int totalPages = totalRows > 0 ? (int)Math.Ceiling(totalRows / (double)pageSize) : 0;

            var pageInfo = new ResponsePaginationInfo
            {
                TotalPagesCount = totalPages,
                TotalRowsCount = totalRows,
                CurrentPageIndex = totalPages > 0 ? pageIndex : 0,
            };

            IQueryable<T> pagedQuery = query.Skip(skip).Take(pageSize);
            return (pagedQuery, pageInfo);
        }

        public async Task<IListWithPaginationInfo<T>> ToListWithPaginationInfoAsync(
            IRequestPaginationInfo paginationInfo, CancellationToken cancellationToken = default)
        {
            (IQueryable<T> pagedQuery, IResponsePaginationInfo pageInfo) = await query
                .ApplyPaginationAsync(paginationInfo, cancellationToken);
            
            return new ListWithPaginationInfo<T>
            {
                PaginationInfo = pageInfo,
                Rows = await pagedQuery.ToListAsync(cancellationToken)
            };
        }
    }
}
