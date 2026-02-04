using Domain.Contracts.StoreDb;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Exceptions;
using Shared.Dtos.OrderModule;

namespace Services.Implementations
{
    public class OrderService(IMapper _mapper,
        IBasketRepository _basketRepository,
        IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string userEmail)
        {
            //1] map address dto to address entity
            var address = _mapper.Map<OrderAddress>(orderRequest.ShipToAddress);
            //2] get OrderItems => BasketId => Basket => BasketItems [Id]
            var basket = await _basketRepository.GetBasketAsync(orderRequest.BasketId)
                ?? throw new BasketNotFoundException(orderRequest.BasketId);
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GenericRepository<Product, int>().GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);
                orderItems.Add(CreateOrderItem(product,item));
            }
            var orderRepo = _unitOfWork.GenericRepository<Order, Guid>();
            //3] get delivery method
            var deliveryMethod = await _unitOfWork.GenericRepository<DeliveryMethod, int>()
                .GetByIdAsync(orderRequest.DeliveryMethodId)
                ?? throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);
            var orderExsists = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(basket.PaymentIntentId));
            if(orderExsists != null)
            {
                return _mapper.Map<OrderResult>(orderExsists);
            } 
            //4] calculate subtotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity); // sum instead of loop
            //5] create order entity => add to db => save changes
            var orderToCreate = new Order(userEmail, address, orderItems, deliveryMethod, subTotal, basket.PaymentIntentId);
            await orderRepo.AddAsync(orderToCreate);
            await _unitOfWork.SaveChangesAsync();
            //6] map order entity to order dto and return
            return _mapper.Map<OrderResult>(orderToCreate);
        }

        private OrderItem CreateOrderItem(Product product, BasketItem item)
        {
            return new OrderItem(
                     new ProductItemOrdered(product.id, product.Name, product.PictureUrl)
                     , item.Quantity, product.Price);
        }

        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GenericRepository<DeliveryMethod, int>()
                .GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodResult>>(deliveryMethods);
        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            var order = await _unitOfWork.GenericRepository<Order, Guid>()
                .GetByIdAsync(new OrderWithIncludesSpecifications(id))
                ?? throw new OrderNotFoundException(id); 
            return _mapper.Map<OrderResult>(order);
        }

        public async Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string userEmail)
        {
            var orders = await _unitOfWork.GenericRepository<Order, Guid>()
                .GetAllAsync(new OrderWithIncludesSpecifications(userEmail));
            return _mapper.Map<IEnumerable<OrderResult>>(orders);
        }
    }
}
