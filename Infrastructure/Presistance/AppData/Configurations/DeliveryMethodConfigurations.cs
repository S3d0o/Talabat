using Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.AppData.Configurations
{
    internal class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(d => d.Price)
                .HasColumnType("decimal(18,4)");
        }
    }
}
