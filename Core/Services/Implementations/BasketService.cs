using Domain.Contracts.StoreDb;
using Domain.Entities.BasketModule;
using Domain.Exceptions;
using Shared.Dtos.BasketModule;

namespace Services.Implementations
{
    public class BasketService(IBasketRepository _basketRepo, IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDto)
        {
            var basket = _mapper.Map<CustomerBasket>(basketDto);
            var CreatedOrUpdatedBasket = await _basketRepo.CreateOrUpdateBasketAsync(basket);
            return CreatedOrUpdatedBasket is null
                ? throw new Exception("Problem Occured While Creating Or Updating Basket")
                : _mapper.Map<BasketDto>(CreatedOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string id)
            => await _basketRepo.DeleteBasketAsync(id);


        public async Task<BasketDto> GetBasketAsync(string id)
        {
            var basket = await _basketRepo.GetBasketAsync(id);
            return basket is null
                ? throw new BasketNotFoundException(id)
                : _mapper.Map<BasketDto>(basket);
        }
    }
}
