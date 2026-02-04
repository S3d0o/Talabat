using Domain.Entities.IdentityModule;
using Domain.Entities.OrderModule;
using ShippingAddress = Domain.Entities.OrderModule.OrderAddress;
public class Order : BaseEntity<Guid>
{
    public Order(string userEmail, ShippingAddress shippingAddress, ICollection<OrderItem> orderItems,
        DeliveryMethod deliveryMethod, decimal subTotal, string paymentIntentId)
    {
        PaymentIntentId = paymentIntentId;
        id = Guid.NewGuid();
        UserEmail = userEmail;
        ShippingAddress = shippingAddress;
        OrderItems = orderItems;
        DeliveryMethod = deliveryMethod;
        SubTotal = subTotal;
    }
    public Order()
    {

    }

    public string UserEmail { get; set; } = string.Empty;
    public ShippingAddress ShippingAddress { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;
    public DeliveryMethod DeliveryMethod { get; set; }
    public int? DeliveryMethodId { get; set; }
    public decimal SubTotal { get; set; }   // SubTotal = OrderItems + quantity * price
    //public decimal Total => SubTotal + DeliveryMethod.Price;
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    public string PaymentIntentId { get; set; } = string.Empty;
}
