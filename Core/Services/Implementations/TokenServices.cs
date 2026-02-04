using Domain.Contracts.IdentityDb;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Dtos.IdentityModule;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace Services.Implementations
{

    public class TokenServices(IConfiguration _config, UserManager<User> _user, IIdentityUnitOfWork _db) : ITokenService
    {
        private readonly int AccessTokenExpirationMinutes = 10;
        private readonly int RefreshTokenExpirationDays = 7;
        private readonly int TheftDetectionWindowMinutes = 5;

        public async Task<TokenDto> GenerateTokensAsync(User user, IList<string> roles, string ipAddress)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (roles == null) roles = new List<string>();
            //roles ??= new List<string>(); // the same as above

            var accessToken = GenerateAccessToken(user, roles);

            // Snapshot the user's security stamp so token is invalidated on password/change-security actions
            var securityStamp = await _user.GetSecurityStampAsync(user);

            // Generate Refresh Token (Random string) => Hash it => Store the hash in DB with userId, expiry, createdByIp
            var refreshToken = GenerateRefreshToken();
            var refreshTokenHash = HashRefreshToken(refreshToken);
            var refreshTokenEntity = CreateRefreshEntity(refreshTokenHash, user.Id, ipAddress, RefreshTokenExpirationDays,securityStamp);
            var expiration = DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes);

            //=> i should reach the refresh token repository from unit of work
            await _db.RefreshTokenRepository.AddRefreshTokenAsync(refreshTokenEntity);

            await _db.SaveChangesAsync();

            return new TokenDto(accessToken, refreshToken, expiration);
        }

        public async Task<TokenDto> RefreshTokenAsync(string refreshToken, string ipAddress)
        {
            var tokenHash = HashRefreshToken(refreshToken);

            // Start transaction to make rotation 
            using var transaction = await _db.BeginTransactionAsync();

            var existingToken = await _db.RefreshTokenRepository.GetRefreshTokenByHashAsync(tokenHash);

            // validate existing token
            if (existingToken == null)
                throw new SecurityTokenException("Invalid refresh token");

            if (!existingToken.IsActive)
                throw new SecurityTokenException("Inactive refresh token");

            if (existingToken.Revoked != null)
                throw new SecurityTokenException("Refresh token already revoked");

            if (existingToken.IsExpired)
                throw new SecurityTokenException("Refresh token expired");

            // If token was already replaced, treat it as used
            if (!string.IsNullOrEmpty(existingToken.ReplacedByTokenHash))
                throw new SecurityTokenException("Refresh token already rotated/replaced");

            // Theft detection: compare last used IP and short time window
            if (existingToken.LastUsed != null)
            {
                var timeSinceLastUse = (DateTime.UtcNow - existingToken.LastUsed.Value).TotalMinutes;
                if (timeSinceLastUse < TheftDetectionWindowMinutes && existingToken.LastUsedByIp != ipAddress)
                    throw new SecurityTokenException("Suspicious refresh token activity detected (possible token theft)");
            }

            // Load user and validate security-stamp (invalidate token if user changed password/security stamp)
            var user = await _user.FindByIdAsync(existingToken.UserId);
            if (user == null)
                throw new SecurityTokenException("Invalid token - User not found");
            var currentStamp = await _user.GetSecurityStampAsync(user);
            if (!string.IsNullOrEmpty(existingToken.SecurityStamp) && existingToken.SecurityStamp != currentStamp)
                throw new SecurityTokenException("Refresh token invalidated due to user security changes");

            // Update last-used metadata (do NOT mark as used for rotation until DB commit)
            existingToken.LastUsed = DateTime.UtcNow;
            existingToken.LastUsedByIp = ipAddress;
            existingToken.RevocationReason = "Rotated"; // clear semantic reason

            // Token Rotation - Generate new refresh token and revoke the old one
            existingToken.Revoked = DateTime.UtcNow;
            existingToken.RevokedByIp = ipAddress;

            var newRefreshToken = GenerateRefreshToken();
            var newRefreshTokenHash = HashRefreshToken(newRefreshToken);

            // Link replacement
            existingToken.ReplacedByTokenHash = newRefreshTokenHash;

            // Create new refresh token entity (not marking LastUsed at creation)
            var newRefreshTokenEntity = CreateRefreshEntity(newRefreshTokenHash, user.Id, ipAddress, RefreshTokenExpirationDays,currentStamp);

            await _db.RefreshTokenRepository.AddRefreshTokenAsync(newRefreshTokenEntity);

            try
            {
                await _db.SaveChangesAsync(); // RowVersion will enforce concurrency
                await transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                // Concurrency indicates token was used/rotated in parallel
                throw new SecurityTokenException("Refresh token already used");
            }

            // Generate new access token
            var roles = await _user.GetRolesAsync(user);
            return new TokenDto
            (
                AccessToken: GenerateAccessToken(user, roles),
                RefreshToken: newRefreshToken,
                AccessTokenExpiresAt: DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes)
            );
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken, string ipAddress, string? reason = null)
        {
            if (string.IsNullOrWhiteSpace(refreshToken)) throw new ArgumentException("refresh token is required", nameof(refreshToken));

            var tokenHash = HashRefreshToken(refreshToken);
            var existingToken = await _db.RefreshTokenRepository.GetRefreshTokenByHashAsync(tokenHash);

            if (existingToken == null) throw new KeyNotFoundException("Refresh token not found");

            if (existingToken.Revoked == null)
            {
                existingToken.Revoked = DateTime.UtcNow;
                existingToken.RevokedByIp = ipAddress;
                existingToken.RevocationReason = reason ?? "RevokedByUserOrAdmin";

                await _db.SaveChangesAsync();
            }
        }

        #region Utilities
        private string GenerateAccessToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name,user.DisplayName)
            };
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured")));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes);

            var token = new JwtSecurityToken
                (
                    issuer: _config["JWT:Issuer"],
                    audience: _config["JWT:Audience"],
                    claims: claims,
                    expires: expiration,
                    signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
            => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private string HashRefreshToken(string raw)
            => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(raw)));

        private RefreshToken CreateRefreshEntity(string Tokenhash, string userId, string ip, int days,string? securityStampSnapshot)
            => new RefreshToken
            {
                TokenHash = Tokenhash,
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(days),
                Created = DateTime.UtcNow,
                CreatedByIp = ip,
                LastUsed = null,
                LastUsedByIp = null,
                // Snapshot security stamp to allow invalidation later
                SecurityStamp = securityStampSnapshot

                // Revoked, RevokedByIp, ReplacedByTokenHash remain null
            };

        #endregion

    }
}
