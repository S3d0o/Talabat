using Domain.Contracts.StoreDb;
using Domain.Entities;
using System.Linq.Expressions;

namespace Services.Specifications
{
    internal abstract class BaseSpecification<TEntity, TKey>
        : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        #region Criteria
        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<TEntity, bool>> Criteria { get; private set; }
        // Helper to combine additional criteria (used when building spec from DTO)
        protected void And(Expression<Func<TEntity, bool>> predicate)
        {
            var param = Criteria.Parameters[0];
            var body = Expression.AndAlso(Criteria.Body, Expression.Invoke(predicate, param));
            Criteria = Expression.Lambda<Func<TEntity, bool>>(body, param);
        }
        #endregion

        #region Include 
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

        public void AddInclude(Expression<Func<TEntity, object>> expression)
        {
            IncludeExpressions.Add(expression);
        }
        #endregion

        #region Sorting 
        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> expression)
        => OrderBy = expression;

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> expression)
            => OrderByDescending = expression;


        #endregion

        #region Pagination 
        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }

        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;
            IsPaginated = true;
        }
        #endregion

    }
}
