using Shared.HelperClasses;

namespace Services.Implementations
{
    public class ServiceManagerWithFactoryDeleget(Func<IProductService> _productFactory,
         Func<IBasketService> _basketFactory, Func<IAuthenticationService> _authFactory,
         Func<ITokenService> _tokenFactory, Func<GetIpHelper.IClientIpProvider> _clientIpFactory,
         Func<IOrderService> _orderFactory, Func<IPaymentService> _paymentFactory,
         Func<ICasheService> _casheFactory) : IServiceManager
    {
        public IProductService ProductService => _productFactory.Invoke();

        public IBasketService BasketService => _basketFactory.Invoke();

        public IAuthenticationService AuthenticationService => _authFactory.Invoke();

        public ITokenService TokenService => _tokenFactory.Invoke();

        public GetIpHelper.IClientIpProvider ClientIpProvider => _clientIpFactory.Invoke();

        public IOrderService OrderService => _orderFactory.Invoke();

        public IPaymentService PaymentService => _paymentFactory.Invoke();

        public ICasheService CasheService => _casheFactory.Invoke();
    }
}
