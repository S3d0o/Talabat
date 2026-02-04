using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Presistance;

public class IdentityStoreDbContext
    : IdentityDbContext<User, IdentityRole, string>
{
    public IdentityStoreDbContext(DbContextOptions<IdentityStoreDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly,
            t=>t.Namespace == "Persistence.IdentityData.IdentityConfigurations");

        builder.Entity<Address>().ToTable("Addresses");
    }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
