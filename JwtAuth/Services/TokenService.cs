using JwtAuth.Dtos.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuth.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthToken GenerateAccessToken(int userId, string userName)
        {
            // Generate a symmetric security key from the secret key in the configuration
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt:Key").Value);
            var creds = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            );

            var expirationMinutes = _configuration.GetSection("Jwt:AccessTokenExpirationMinutes").Value;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // jti is the JWT ID, a unique identifier for the token
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()), // sub is the subject of the token, typically the user ID
                new Claim(JwtRegisteredClaimNames.UniqueName, userName), // unique name is often the username or email
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(expirationMinutes)),
                signingCredentials: creds
            );

            return new AuthToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token), // Serialize the token to a string
                ExpiresAt = token.ValidTo // Set the expiration time of the token
            };
        }

        public AuthToken GenerateRefreshToken(int userId, string userName)
        {
            var refreshTokenExpirationDays = int.Parse(_configuration.GetSection("Jwt:RefreshTokenExpirationDays").Value);

            return new AuthToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.Now.AddDays(refreshTokenExpirationDays) // Refresh tokens typically last longer than access tokens
            };
        }

        //public async Task RevokeRefreshToken(AppUser user, string token)
        //{
        //    var refreshToken = _context.RefreshTokens
        //        .FirstOrDefault(rt => rt.Token == token && rt.UserId == user.Id);

        //    if (refreshToken == null)
        //        throw new Exception("Refresh token not found");
        //    refreshToken.IsRevoked = true;

        //    await _context.SaveChangesAsync();
        //}

    }
}
