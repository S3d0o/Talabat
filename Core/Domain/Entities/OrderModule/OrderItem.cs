namespace Domain.Entities.OrderModule
{
    public class OrderItem : BaseEntity<Guid>
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductItemOrdered productItemOrdered, int quantity, decimal price)
        {
            ProductItemOrdered = productItemOrdered;
            Quantity = quantity;
            Price = price;
        }

        public ProductItemOrdered ProductItemOrdered { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
