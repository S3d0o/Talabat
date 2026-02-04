using Shared.Dtos.OrderModule;

namespace Service.Abstraction.Contracts
{
    public interface IOrderService
    {
        //Gets an order by its ID
        Task<OrderResult> GetOrderByIdAsync(Guid id);
        //Get all orders by Email
        Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string userEmail);
        //Create an order
        Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string userEmail);
        //Get delivery methods
        Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync();
    }
}
