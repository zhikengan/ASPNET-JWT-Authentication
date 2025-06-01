using JwtAuth.Interfaces;

namespace JwtAuth.Models
{
    public class AppUser : IAuditable
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; } // salt means the password is hashed with a unique salt added at the end of the password
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property for related UserSessions
        public ICollection<UserSession> UserSessions { get; set; }
    }
}
