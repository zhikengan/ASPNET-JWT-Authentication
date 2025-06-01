using JwtAuth.Interfaces;

namespace JwtAuth.Models
{
    public class UserSession : IAuditable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; } // navigation property

        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } // indicates if the session is revoked

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
