namespace Service.Abstraction.Contracts
{
    public interface ICasheService
    {
        Task<string?> GetCacheValueAsync(string key);
        Task SetCacheValueAsync(string key, object value, TimeSpan? expiration = null);
    }
}
