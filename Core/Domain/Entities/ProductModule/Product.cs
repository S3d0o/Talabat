namespace Domain.Entities.ProductModule
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public ProductType ProductType { get; set; } = null!;
        public int TypeId { get; set; } // fk
        public ProductBrand? ProductBrand { get; set; } = null!;
        public int? BrandId { get; set; } // fk
    }
}
