using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Contracts.StoreDb
{
    public interface IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        //GetAll
        Task<IEnumerable<TEntity>> GetAllAsync(bool AsNoTracking = false);
        //GetById
        Task<TEntity?> GetByIdAsync(Tkey id);
        //Add
        Task AddAsync(TEntity entity);
        //Delete
        void Delete(TEntity entity);
        //Update
        void Update(TEntity entity);
        Task<List<TEntity>> FindAsync(
                 Expression<Func<TEntity, bool>> predicate,
                 bool asNoTracking = false);

        #region Specifications
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, Tkey> specifications);
        Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, Tkey> specifications);
        Task<int> CountAsync(ISpecifications<TEntity, Tkey> specifications);
        #endregion
    }
}
