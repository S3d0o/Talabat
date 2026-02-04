namespace Domain.Contracts.StoreDb
{
    public interface IUnitOfWork
    {
        // SaveChanges
        Task<int> SaveChangesAsync();

        IGenericRepository<TEntity, Tkey> GenericRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>;
    }
}
