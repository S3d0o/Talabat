using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.IdentityModule
{
    public class RefreshToken
    {
        public int Id { get; set; } 
        public string TokenHash { get; set; } = null!; // SHA256 hash of token
        public string UserId { get; set; } = null!;     // FK to AspNetUsers
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; } = null!;
        public DateTime? Revoked { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByTokenHash { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? LastUsed { get; set; }
        public string? LastUsedByIp { get; set; }
        public string? RevocationReason { get; set; }
        public string? SecurityStamp { get; set; }

        // THIS makes concurrency check possible
        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
    }
}
