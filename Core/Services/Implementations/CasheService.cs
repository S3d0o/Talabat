
using Domain.Contracts.StoreDb;

namespace Services.Implementations
{
    public class CasheService(ICasheRepository _casheRepository) : ICasheService
    {
        public async Task<string?> GetCacheValueAsync(string key)
        => await _casheRepository.GetCasheValueAsync(key);

        public async Task SetCacheValueAsync(string key, object value, TimeSpan? expiration = null)
        => await _casheRepository.SetCasheValueAsync(key, value, expiration);
    }
}
