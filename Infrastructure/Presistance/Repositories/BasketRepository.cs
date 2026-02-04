using Domain.Contracts.StoreDb;
using Domain.Entities.BasketModule;
using StackExchange.Redis;
using System.Text.Json;

namespace Presistance.Repositories
{
    public class BasketRepository(IConnectionMultiplexer _connection) : IBasketRepository
    {
        private readonly IDatabase _database = _connection.GetDatabase();
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLeave = null)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            var result = await _database.StringSetAsync(basket.Id, jsonBasket, timeToLeave ?? TimeSpan.FromDays(30));
            return result ? await GetBasketAsync(basket.Id) : null;
        }

        public async Task<bool> DeleteBasketAsync(string id)
        => await _database.KeyDeleteAsync(id);

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
           var basket = await _database.StringGetAsync(id);
           return (basket.IsNullOrEmpty) ?  null : JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }
    }
}