using Shared.Dtos.BasketModule;

namespace Service.Abstraction.Contracts
{
    public interface IBasketService
    {
        //Get Basket By Id
        public Task<BasketDto> GetBasketAsync(string id);
        //Create Or Update Basket
        public Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDto);
        //Delete Basket By Id
        public Task<bool> DeleteBasketAsync(string id);
    }
}
