using Domain.Contracts;
using E_Commerce.Middlewares;

namespace E_Commerce.Extensions
{
    public static class WebApplicationExtensions
    {
        public async static Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        {
             using var scope = app.Services.CreateScope();
            await scope.ServiceProvider.GetRequiredService<IDataSeeding>().SeedDataAsync(); // Seed the data on app startup
            await scope.ServiceProvider.GetRequiredService<IDataSeeding>().SeedIdentityDataAsync(); // Seed the identity data on app startup
            return app;
        }
        public static WebApplication AddExceptionHandlingMiddleware(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            return app;
        }
        public static WebApplication UseSawggerMiddleware(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
