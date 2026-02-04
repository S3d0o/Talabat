using Domain.Contracts.StoreDb;
using Microsoft.EntityFrameworkCore;
using Presistance.Data;

namespace Presistance.HelperClasses
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity, Tkey>(IQueryable<TEntity> inputQuery,
            ISpecifications<TEntity, Tkey> specifications) where TEntity : BaseEntity<Tkey>
        {
            var query = inputQuery;

            // Apply criteria
            if (specifications.Criteria is not null)
                query = query.Where(specifications.Criteria);

            // Apply sorting
            if (specifications.OrderBy != null)
                query = query.OrderBy(specifications.OrderBy);

            if (specifications.OrderByDescending != null)
                query = query.OrderByDescending(specifications.OrderByDescending);

            // Apply includes
            if (specifications.IncludeExpressions != null && specifications.IncludeExpressions.Any())
            {
                foreach (var includeExpression in specifications.IncludeExpressions)
                    query = query.Include(includeExpression);

                //query = specifications.IncludeExpressions.Aggregate(query,(currentQuery,expression)=>currentQuery.Include(expression));
            }

            // Apply pagination
            if(specifications.IsPaginated)
                query = query.Skip(specifications.Skip).Take(specifications.Take);

            return query;
        }

    }
}
