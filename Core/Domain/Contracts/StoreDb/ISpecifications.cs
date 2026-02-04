using System.Linq.Expressions;

namespace Domain.Contracts.StoreDb
{
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        // The criteria to filter entities
        public Expression<Func<TEntity, bool>> Criteria { get; }

        // The list of related entities to include
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; }

        // OrderBy
        public Expression<Func<TEntity, object>>? OrderBy { get; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; }

        // pagination [Take - Skip]
        public int Take { get; }
        public int Skip { get; }
        public bool IsPaginated { get; }
    }
}
