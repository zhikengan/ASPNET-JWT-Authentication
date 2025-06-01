using System.Security.Claims;

namespace JwtAuth.Helpers
{
    public static class IdentityUtils
    {
        public static string GetUsername(HttpContext context)
        {
            if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
            {
                return context.User.Identity.Name ?? string.Empty;
            }
            return string.Empty;
        }

        public static int GetUserId(HttpContext context)
        {
            if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
            {
                var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
                {
                    return userId;
                }
            }
            return -1; // or throw an exception if preferred
        }
    }
}
