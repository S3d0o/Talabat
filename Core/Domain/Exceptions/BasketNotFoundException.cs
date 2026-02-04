namespace Domain.Exceptions
{
    public class BasketNotFoundException : NotFoundException
    {
        public BasketNotFoundException(string id) : base($"Basket with id '{id}' was not found.")
        {
            
        }
    }
}
