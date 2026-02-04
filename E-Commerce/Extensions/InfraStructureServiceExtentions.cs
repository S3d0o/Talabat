using Domain.Contracts;
using Domain.Contracts.IdentityDb;
using Domain.Contracts.StoreDb;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistance.Repositories;
using Presistance.Data;
using Presistance.IdentityDb.IdentityRepositories;
using Presistance.Repositories;
using Service.Abstraction.Contracts;
using Services.Implementations;
using StackExchange.Redis;
using Stripe;
using System.Text;
using static Shared.HelperClasses.GetIpHelper;

namespace E_Commerce.Extensions
{
    public static class InfraStructureServiceExtentions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
             {
                 options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
             });
            services.AddDbContext<IdentityStoreDbContext>(options =>
             {
                 options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
             });
            services.AddScoped<IDataSeeding, DataSeeding>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
            services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
            });
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ITokenService, TokenServices>();
            // to get the client ip address
            services.AddHttpContextAccessor();
            services.AddScoped<IClientIpProvider, ClientIpProvider>();
            services.AddScoped<ICasheRepository, CasheRepository>();


            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<IdentityStoreDbContext>();
            services.ValidJwt(configuration);

            #region Stripe 
            var stripeKey = configuration.GetValue<string>("StripeSettings:SecretKey")
                 ?? throw new Exception("Stripe key missing");

            StripeConfiguration.ApiKey = stripeKey;

            // Register payment service
            services.AddScoped<IPaymentService, PaymentService>();

            #endregion
            return services;
        }
       

        // JWT Validation
        public static IServiceCollection ValidJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
                    
                };
            });
            services.AddAuthorization();
            return services;
        }
    }
}
