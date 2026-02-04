namespace Shared.Dtos.IdentityModule
{
    public record TokenDto(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAt);
}
