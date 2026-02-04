using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Domain.Entities.OrderModule;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Presistance.Data
{
    public class DataSeeding(StoreDbContext _dbContext
        , UserManager<User> _userManager
        , RoleManager<IdentityRole> _roleManager) : IDataSeeding
    {
        public async Task SeedDataAsync()
        {
            try
            {
                var pendingMigration = await _dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigration.Any())
                {
                    await _dbContext.Database.MigrateAsync();
                }
                if (!_dbContext.ProductBrands.Any())
                {
                    var ProductBrandsData = File.OpenRead("..\\Infrastructure\\Presistance\\AppData\\DataSeed\\brands.json");
                    // json to C# object (deserialization)
                    var productBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(ProductBrandsData);
                    if (productBrands is not null && productBrands.Any())
                        await _dbContext.ProductBrands.AddRangeAsync(productBrands);
                }
                if (!_dbContext.ProductTypes.Any())
                {
                    var ProductTypesData = File.OpenRead("..\\Infrastructure\\Presistance\\AppData\\DataSeed\\types.json");
                    // json to C# object (deserialization)
                    var productTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(ProductTypesData);
                    if (productTypes is not null && productTypes.Any())
                        await _dbContext.ProductTypes.AddRangeAsync(productTypes);
                }
                if (!_dbContext.Products.Any())
                {
                    var ProductsData = File.OpenRead("..\\Infrastructure\\Presistance\\AppData\\DataSeed\\products.json");
                    // json to C# object (deserialization)
                    var products = await JsonSerializer.DeserializeAsync<List<Product>>(ProductsData);
                    if (products is not null && products.Any())
                        await _dbContext.Products.AddRangeAsync(products);
                }
                if(!_dbContext.DeliveryMethods.Any())
                {
                    var deliveryMethodData = File.OpenRead("..\\Infrastructure\\Presistance\\AppData\\DataSeed\\delivery.json");
                    // json to C# object (deserialization)
                    var deliveryMethods = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(deliveryMethodData);
                    if (deliveryMethods is not null && deliveryMethods.Any())
                        await _dbContext.DeliveryMethods.AddRangeAsync(deliveryMethods);
                }
                _dbContext.SaveChanges();
            }

            catch (Exception ex)
            {
                // handle exception
            }
        }

        public async Task SeedIdentityDataAsync()
        {
            try
            {
                // Seed Roles
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                   await _roleManager.CreateAsync(new IdentityRole("Member"));
                }
                // Seed Users
                if (!_userManager.Users.Any())
                {
                    var adminUser = new User
                    {
                        DisplayName = "Admin",
                        UserName = "AdminUser",
                        Email = "Admin@gmail.com",
                        PhoneNumber = "012345678890",

                    };

                    var MemberUser = new User
                    {
                        DisplayName = "Member",
                        UserName = "MemberUser",
                        Email = "Member@gmail.com",
                        PhoneNumber = "098765432123",
                    };
                    await _userManager.CreateAsync(adminUser, "Admin123$");
                    await _userManager.AddToRoleAsync(adminUser, "Admin");

                    await _userManager.CreateAsync(MemberUser, "Member123$");
                    await _userManager.AddToRoleAsync(MemberUser, "Member");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
