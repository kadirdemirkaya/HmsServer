using Hsm.Domain.Models.Page;
using Microsoft.EntityFrameworkCore;

namespace Hsm.Application.Extensions
{
    public static class PagingExtensions
    {
        public static async Task<PageResponse<T>> GetPage<T>(this IQueryable<T> query, int? currentPage, int? pageSize)
            where T : class
        {
            var count = query.Count();
            Page paging = new(currentPage ?? 1, pageSize ?? 10, count);

            var data = await query.AsNoTracking()
                                  .Skip(paging.Skip)
                                  .Take(paging.PageSize)
                                  .ToListAsync();

            var result = new PageResponse<T>(data, paging);

            return result;
        }
    }
}
