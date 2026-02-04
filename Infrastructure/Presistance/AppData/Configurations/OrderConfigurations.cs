using Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.AppData.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, a => a.WithOwner());
            builder.HasMany(p => p.OrderItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(o => o.PaymentStatus)
                .HasConversion(ps => ps.ToString(),
                ps => Enum.Parse<OrderPaymentStatus>(ps));
            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
            builder.Property(o => o.SubTotal)
                .HasColumnType("decimal(18,4)");
        }
    }
}
