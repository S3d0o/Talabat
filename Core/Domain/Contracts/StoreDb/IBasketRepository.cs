using Domain.Entities.BasketModule;

namespace Domain.Contracts.StoreDb
{
    public interface IBasketRepository
    {
        public Task<CustomerBasket?> GetBasketAsync(string id);
        public Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket,TimeSpan? timeToLeave = null);
        public Task<bool> DeleteBasketAsync(string id);
    }
}
