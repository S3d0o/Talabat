using Service.Abstraction.Contracts;
using Services;
using Services.Implementations;
using Shared.HelperClasses;

namespace E_Commerce.Extensions
{
    public static class CoreServicesExtentions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(AssemblyReference).Assembly);
            services.AddScoped<IServiceManager, ServiceManagerWithFactoryDeleget>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<Func<IProductService>>(provider =>
            () => provider.GetRequiredService<IProductService>()
            );

            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<Func<IBasketService>>(provider =>
            () => provider.GetRequiredService<IBasketService>()
            );

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<Func<IAuthenticationService>>(provider =>
            () => provider.GetRequiredService<IAuthenticationService>()
            );

            services.AddScoped<ITokenService, TokenServices>();
            services.AddScoped<Func<ITokenService>>(provider =>
            () => provider.GetRequiredService<ITokenService>()
            );

            services.AddScoped<GetIpHelper.IClientIpProvider, GetIpHelper.ClientIpProvider>();
            services.AddScoped<Func<GetIpHelper.IClientIpProvider>>(provider =>
            () => provider.GetRequiredService<GetIpHelper.IClientIpProvider>()
            );

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<Func<IOrderService>>(provider =>
            () => provider.GetRequiredService<IOrderService>()
            );

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<Func<IPaymentService>>(provider =>
            () => provider.GetRequiredService<IPaymentService>()
            );

            services.AddScoped<ICasheService, CasheService>();
            services.AddScoped<Func<ICasheService>>(provider =>
            () => provider.GetRequiredService<ICasheService>());

            return services;
        }

    }
}
