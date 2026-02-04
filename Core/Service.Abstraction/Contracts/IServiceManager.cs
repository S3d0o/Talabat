using static Shared.HelperClasses.GetIpHelper;

namespace Service.Abstraction.Contracts
{
    public interface IServiceManager
    {
        public IProductService ProductService { get; }
        public IBasketService BasketService { get; }
        public IAuthenticationService AuthenticationService { get; }
        public ITokenService TokenService { get; }
        public IClientIpProvider ClientIpProvider { get; }
        public IOrderService OrderService { get; }
        public IPaymentService PaymentService { get; }
        public ICasheService CasheService { get; }
    }
}
