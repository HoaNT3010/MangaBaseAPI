using Microsoft.EntityFrameworkCore;

namespace MangaBaseAPI.Application.Common.Utilities
{
    public class PagedList<T> where T : class
    {
        private PagedList(
            List<T> items,
            int pageNumber,
            int pageSize,
            int totalCount)
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public List<T> Items { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public bool HasNextPage => PageNumber * PageSize < TotalCount;

        public bool HasPreviousPage => PageNumber > 1;

        public static async Task<PagedList<T>> CreateAsync(
            IQueryable<T> query,
            int pageNumber,
            int pageSize)
        {
            int totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new(items, pageNumber, pageSize, totalCount);
        }
    }
}
