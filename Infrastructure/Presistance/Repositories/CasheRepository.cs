using Domain.Contracts.StoreDb;
using StackExchange.Redis;
using System.Text.Json;

namespace Persistance.Repositories
{
    public class CasheRepository(IConnectionMultiplexer _connection) : ICasheRepository
    {
        private readonly IDatabase _database = _connection.GetDatabase();
        public async Task<string> GetCasheValueAsync(string key)
        {
            var value = await _database.StringGetAsync(key);
            return value.IsNullOrEmpty ? default :  value;
        }

        public async Task SetCasheValueAsync(string key, object value, TimeSpan? expiration = null)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, serializedValue, expiration);
        }
    }
}
