using Microsoft.AspNetCore.Authorization;
using Shared.Dtos.IdentityModule;
using Shared.Dtos.OrderModule;
using System.Security.Claims;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManager _serviceManager) : ApiController
    {
        //Post: api/authentication/Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDto>> RegisterAsync(RegisterDto registerDto)
            => Ok(await (_serviceManager.AuthenticationService.RegisterAsync(registerDto)));

        //Post : api/authentication/Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>> LoginAsync(LoginDto loginDto)
            => Ok(await (_serviceManager.AuthenticationService.LoginAsync(loginDto)));

        //Post : api/authentication/RefreshToken
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<TokenDto>> RefreshToken([FromBody] string refreshToken)
        {
            var ip = _serviceManager.ClientIpProvider.GetClientIp();
            var tokens = await _serviceManager.TokenService.RefreshTokenAsync(refreshToken, ip);
            return Ok(tokens);
        }

        //Post : api/authentication/RevokeToken
        [HttpPost("RevokeToken")]
        public async Task<ActionResult> RevokeToken([FromBody] string refreshToken)
        {
            var ip = _serviceManager.ClientIpProvider.GetClientIp();
            await _serviceManager.TokenService.RevokeRefreshTokenAsync(refreshToken, ip, "Revoked by user");
            return NoContent();
        }

        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExistAsync([FromQuery] string email)
            => Ok(await _serviceManager.AuthenticationService.CheckEmailExistAsync(email));

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResultDto>> GetCurrentUserAsync()
        {
            var  userEmail = User.FindFirstValue(ClaimTypes.Email)!;
            var user = await _serviceManager.AuthenticationService.GetCurrentUserAsync(userEmail);
            return Ok(user);
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetUserAddressAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email)!;
            var address = await _serviceManager.AuthenticationService.GetUserAddressAsync(userEmail);
            return Ok(address);
        }
        [Authorize]
        [HttpPut]
        public async Task<ActionResult<AddressDto>> UpdateUserAddressAsync(AddressDto addressDto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email)!;
            var updatedAddress = await _serviceManager.AuthenticationService
                .UpdateUserAddressAsync(userEmail, addressDto);
            return Ok(updatedAddress);
        }

    }
}
