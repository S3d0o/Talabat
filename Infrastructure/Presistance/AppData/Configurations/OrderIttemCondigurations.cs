using Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.AppData.Configurations
{
    internal class OrderIttemCondigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(o => o.Price)
                .HasColumnType("decimal(18,4)");
            builder.OwnsOne(o => o.ProductItemOrdered, p=>p.WithOwner());
        }
    }
}
