using AutoMapper;
using Domain.Contracts.IdentityDb;
using Domain.Contracts.StoreDb;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using static Shared.HelperClasses.GetIpHelper;

namespace Services.Implementations
{
    public class ServiceManager(
          IUnitOfWork _unitOfWork
        , IMapper _mapper
        , IBasketRepository _basketRepository
        , UserManager<User> _userManager
        , ITokenService _tokenService
        , IClientIpProvider _clientIpProvider
        , IConfiguration _config
        , UserManager<User> _user
        , IIdentityUnitOfWork _db
        , IHttpContextAccessor _contextAccessor) /*: IServiceManager*/
    {
        private readonly Lazy<IProductService> _productService = new Lazy<IProductService>(()=> new ProductService(_unitOfWork,_mapper));
        private readonly Lazy<IBasketService> _basketService = new Lazy<IBasketService>(() => new BasketService(_basketRepository,_mapper));
        private readonly Lazy<IAuthenticationService> _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(_userManager, _tokenService, _clientIpProvider, _mapper));
        private readonly Lazy<ITokenService> _tokenService = new Lazy<ITokenService>(() => new TokenServices(_config,_user,_db));
        private readonly Lazy<IClientIpProvider> _clientIp = new Lazy<IClientIpProvider>(() => new ClientIpProvider(_contextAccessor));
        private readonly Lazy<IOrderService> _orderService = new Lazy<IOrderService>(() => new OrderService(_mapper, _basketRepository, _unitOfWork));
        private readonly Lazy<IPaymentService> _paymentService = new Lazy<IPaymentService>(() => new PaymentService(_basketRepository, _unitOfWork, _mapper, _config));
        
        public IProductService ProductService => _productService.Value;
        public IBasketService BasketService => _basketService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public ITokenService TokenService => _tokenService.Value;
        public IClientIpProvider ClientIpProvider => _clientIp.Value;
        public IOrderService OrderService => _orderService.Value;
        public IPaymentService PaymentService => _paymentService.Value;
    }
}
