using Domain.Contracts;
using E_Commerce.Extensions;
using E_Commerce.Factories;
using E_Commerce.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presistance.Data;
using Presistance.Repositories;
using Service.Abstraction.Contracts;
using Services;
using Services.Implementations;
using System.Reflection;

namespace E_Commerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region DI Container

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Web API Services
            builder.Services.AddWebApiServices(builder.Configuration);

            // Infrastructure Services
            builder.Services.AddInfrastructureServices(builder.Configuration);
            // Core Services
            builder.Services.AddCoreServices();

            #endregion

            #region Pipelines - Middlewares 
            var app = builder.Build();
            await app.SeedDatabaseAsync();

            app.AddExceptionHandlingMiddleware();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSawggerMiddleware();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run(); 
            #endregion
        }
    }
}
