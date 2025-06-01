using System.ComponentModel.DataAnnotations;

namespace JwtAuth.Dtos.Request
{
    public class LogoutRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
