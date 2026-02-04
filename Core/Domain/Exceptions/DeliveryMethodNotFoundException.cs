namespace Domain.Exceptions
{
    public class DeliveryMethodNotFoundException : NotFoundException
    {
        public DeliveryMethodNotFoundException(int id) : base($"Delivery Method with {id} not Found")
        {
        }
    }
}
