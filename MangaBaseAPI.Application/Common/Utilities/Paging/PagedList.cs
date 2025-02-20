using Microsoft.EntityFrameworkCore;

namespace MangaBaseAPI.Application.Common.Utilities.Paging
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

        public int TotalCount { get; set; }

        public bool HasNextPage => PageNumber * PageSize < TotalCount;

        public bool HasPreviousPage => PageNumber > 1;

        public static async Task<PagedList<T>> CreateAsync(
            IQueryable<T> query,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            int totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new(items, pageNumber, pageSize, totalCount);
        }

        public static PagedList<T> Create(
            IQueryable<T> query,
            int pageNumber,
            int pageSize)
        {
            int totalCount = query.Count();
            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return new(items, pageNumber, pageSize, totalCount);
        }

        public static PagedList<T> Create(
            List<T> items,
            int pageNumber,
            int pageSize,
            int totalCount)
        {
            return new(items, pageNumber, pageSize, totalCount);
        }
    }
}
