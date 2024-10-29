using Microsoft.EntityFrameworkCore;

namespace MangaBaseAPI.Domain.Abstractions.Specification
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity>(
            IQueryable<TEntity> inputQueryable,
            Specification<TEntity> specification)
            where TEntity : class
        {
            IQueryable<TEntity> queryable = inputQueryable;

            if (specification.Criteria is not null)
            {
                queryable = queryable.Where(specification.Criteria);
            }

            queryable = specification.IncludeExpressions.Aggregate(
                queryable,
                (current, includeExpression) =>
                    current.Include(includeExpression));

            if (specification.OrderByExpression is not null)
            {
                queryable = queryable.OrderBy(specification.OrderByExpression);
            }
            else if (specification.OrderByDescendingExpression is not null)
            {
                queryable = queryable.OrderByDescending(specification.OrderByDescendingExpression);
            }

            if (specification.IsSplitQuery)
            {
                queryable = queryable.AsSplitQuery();
            }

            if (IsPageSizeValid(specification.PageSize) &&
                IsPageNumberValid(specification.PageNumber))
            {
                int skip = (int)(specification.PageNumber - 1)! * specification.PageSize!.Value;
                queryable = queryable.Skip(skip)
                    .Take(specification.PageSize.Value);
            }

            if (specification.AsTracking)
            {
                queryable = queryable.AsTracking();
            }
            else
            {
                queryable = queryable.AsNoTracking();
            }

            return queryable;
        }

        private static bool IsPageSizeValid(int? pageSize)
            => pageSize.HasValue && pageSize.Value > 0;

        private static bool IsPageNumberValid(int? pageNumber)
            => pageNumber.HasValue && pageNumber.Value > 0;
    }
}
