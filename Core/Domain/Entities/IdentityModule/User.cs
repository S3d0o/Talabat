using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.IdentityModule
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public Address Adress { get; set; } = null!;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
