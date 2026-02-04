

using Domain.Contracts.StoreDb;
using Presistance.Data;
using Presistance.HelperClasses;
using System.Linq.Expressions;

namespace Presistance.Repositories
{
    internal class GenericRepository<TEntity, Tkey>(StoreDbContext _dbContext)
        : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool AsNoTracking = false)
        => AsNoTracking
            ? await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync()
            : await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(Tkey id)
        => await _dbContext.Set<TEntity>().FindAsync(id);

        public async Task AddAsync(TEntity entity)
        => await _dbContext.Set<TEntity>().AddAsync(entity);

        public void Update(TEntity entity)
        => _dbContext.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity)
        => _dbContext.Set<TEntity>().Remove(entity);

        public async Task<List<TEntity>> FindAsync(
                 Expression<Func<TEntity, bool>> predicate,
                 bool asNoTracking = false)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.Where(predicate).ToListAsync();
        }


        #region Specifications 
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, Tkey> specifications)
        => await SpecificationEvaluator.GetQuery(_dbContext.Set<TEntity>(), specifications).ToListAsync();


        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, Tkey> specifications)
        => await SpecificationEvaluator.GetQuery(_dbContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();

        public async Task<int> CountAsync(ISpecifications<TEntity, Tkey> specifications)
        => await SpecificationEvaluator.GetQuery(_dbContext.Set<TEntity>(), specifications).CountAsync();

        #endregion
    }
}
