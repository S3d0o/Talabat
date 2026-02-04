namespace Domain.Contracts.StoreDb
{
    public interface ICasheRepository
    {
        Task<string> GetCasheValueAsync(string key);
        Task SetCasheValueAsync(string key, object value, TimeSpan? expiration = null);
    }
}
