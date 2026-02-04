using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.BasketModule
{
    public record BasketItemDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        [Range(1, 99)]
        public int Quantity { get; init; }
        public string PictureUrl { get; init; } = string.Empty;
    }
}