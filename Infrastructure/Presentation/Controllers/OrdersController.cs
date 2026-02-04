using Shared.Dtos.OrderModule;
using System.Security.Claims;

namespace Presentation.Controllers
{
    public class OrdersController(IServiceManager _serviceManager) : ApiController
    {
        //CreateOrder
        [HttpPost]
        public async Task<ActionResult<OrderResult>> CreateOrderAsync(OrderRequest orderRequest  )
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order =  await _serviceManager.OrderService.CreateOrderAsync(orderRequest, email);
            return Ok(order);
        }
        // Gets an order by its ID
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderResult>> GetOrderByIdAsync(Guid id)
        {
            var order = await _serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
        //Get all orders by Email
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResult>>> GetAllOrdersByEmailAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _serviceManager.OrderService.GetOrdersByEmailAsync(email);
            return Ok(orders);
        }
        //Get delivery methods
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodResult>>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _serviceManager.OrderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }
    }
}
