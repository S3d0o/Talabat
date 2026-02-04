
using Domain.Contracts.StoreDb;
using Presistance.Data;
using System.Collections.Concurrent;

namespace Presistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly ConcurrentDictionary<string, object> _repositories; // key: "TEntity_Tkey", value: GenericRepository<TEntity,Tkey>

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new();
        }

        public async Task<int> SaveChangesAsync()
        =>  await _dbContext.SaveChangesAsync();

            // i will use the ConcurrentDictionary to make it thread safe and to make it simpler
        public IGenericRepository<TEntity, Tkey> GenericRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        => (IGenericRepository<TEntity, Tkey>) _repositories
            .GetOrAdd(typeof(TEntity).Name, (_) => new GenericRepository<TEntity, Tkey>(_dbContext));

            //var key = typeof(TEntity).Name; // Key based on TEntity name
            //if (!_repositories.ContainsKey(key))
            //{
            //    var repository = new GenericRepository<TEntity, Tkey>(_dbContext);
            //    _repositories[key] = repository; // Store the repository instance
            //}
            //return (IGenericRepository<TEntity, Tkey>) _repositories[key]; // casting to the correct type
    }
}
