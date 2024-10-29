using System.Linq.Expressions;

namespace MangaBaseAPI.Domain.Abstractions.Specification
{
    public abstract class Specification<TEntity>
        where TEntity : class
    {
        protected Specification(
            Expression<Func<TEntity, bool>>? criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<TEntity, bool>>? Criteria { get; }

        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

        public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }

        public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

        public bool IsSplitQuery { get; protected set; }

        public int? PageSize { get; private set; }

        public int? PageNumber { get; private set; }

        public bool AsTracking { get; protected set; }

        protected void AddInclude(
            Expression<Func<TEntity, object>> includeExpression)
            => IncludeExpressions.Add(includeExpression);

        protected void AddOrderBy(
            Expression<Func<TEntity, object>> orderByExpression)
            => OrderByExpression = orderByExpression;

        protected void AddOrderByDescending(
            Expression<Func<TEntity, object>> orderByDescendingExpression)
            => OrderByDescendingExpression = orderByDescendingExpression;

        protected void ApplyPaging(
            int pageSize,
            int pageNumber)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
