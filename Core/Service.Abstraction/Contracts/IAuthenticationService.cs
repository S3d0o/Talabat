using Shared.Dtos.IdentityModule;
using Shared.Dtos.OrderModule;

namespace Service.Abstraction.Contracts
{
    public interface IAuthenticationService
    {
        public Task<UserResultDto> LoginAsync(LoginDto loginDto);
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        Task<UserResultDto> GetCurrentUserAsync(string userEmail);
        Task<bool> CheckEmailExistAsync(string userEmail);
        Task<AddressDto> GetUserAddressAsync(string userEmail);
        Task<AddressDto> UpdateUserAddressAsync(string userEmail, AddressDto addressDto);
    }
}
