namespace Shared.Dtos.BasketModule
{
    public class BasketDto
    {
        public string Id { get; init; } = string.Empty;
        public ICollection<BasketItemDto> Items { get; init; } = [];
        public string? PaymentIntentId { get; init; }
        public string? ClientSecret { get; init; }
        public decimal? ShippingPrice { get; init; } // Delivery.Price
        public int? DeliveryMethodId { get; init; }
    }
}
