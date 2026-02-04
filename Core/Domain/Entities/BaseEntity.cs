namespace Domain.Entities
{
    public class BaseEntity<Tkey>
    {
        public Tkey id { get; set; } = default!;
    }
}
