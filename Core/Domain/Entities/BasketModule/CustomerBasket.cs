namespace Domain.Entities.BasketModule
{
    public class CustomerBasket
    {
        public string Id { get; set; } = string.Empty;
        public IEnumerable<BasketItem> Items { get; set; } = new List<BasketItem>();
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public decimal? ShippingPrice { get; set; } // Delivery.Price
        public int? DeliveryMethodId { get; set; }
    }
}
