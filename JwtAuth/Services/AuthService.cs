using JwtAuth.Dtos.Internal;
using JwtAuth.Models;
using JwtAuth.Repositories;
using JwtAuth.Services.Helpers;

namespace JwtAuth.Services
{
    public class AuthService
    {

        private readonly AppUserRepository _userRepo;
        private readonly UserSessionRepository _userSessionRepo;

        public AuthService(AppUserRepository userRepo, UserSessionRepository userSessionRepo)
        {
            _userRepo = userRepo;
            _userSessionRepo = userSessionRepo;
        }

        public async Task<AppUser?> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username and password cannot be empty");
            }
            var user = await _userRepo.GetUserByUsernameAsync(username);

            // Verify password
            if (user == null || !CryptoHelper.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                return null; // Invalid password or user not found
            }

            return user; // Authentication successful
        }

        public async Task<AppUser> RegisterAsync(AppUser appUser, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty", nameof(password));
            }

            if (await _userRepo.GetUserByUsernameAsync(appUser.UserName) != null)
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Password hashing
            var salt = CryptoHelper.GenerateSalt();

            appUser.PasswordSalt = salt;
            appUser.PasswordHash = CryptoHelper.HashPassword(password, salt);

            var createdUser = await _userRepo.CreateUserAsync(appUser);

            return createdUser;
        }

        public async Task<UserSession> CreateSessionAsync(AppUser appUser, AuthToken refreshToken)
        {
            if (appUser == null || refreshToken == null)
            {
                throw new ArgumentNullException("AppUser and AuthToken cannot be null");
            }

            var userSession = new UserSession
            {
                UserId = appUser.Id,
                RefreshToken = refreshToken.Token,
                ExpiresAt = refreshToken.ExpiresAt
            };

            return await _userSessionRepo.AddUserSessionAsync(userSession);
        }

        public async Task<bool> RevokeAllSessionsAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero", nameof(userId));
            }
            return await _userSessionRepo.RevokeAllUserSessionsAsync(userId);
        }

        public async Task<bool> RevokeSessionAsync(int userId, string refreshToken)
        {

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new ArgumentException("Refresh token cannot be empty", nameof(refreshToken));
            }

            return await _userSessionRepo.RevokeUserSessionAsync(userId, refreshToken);
        }

        public async Task<UserSession?> GetUserSessionAsync(int userId, string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new ArgumentException("Refresh token cannot be empty", nameof(refreshToken));
            }
            return await _userSessionRepo.GetUserSessionByRefreshTokenAsync(userId, refreshToken);
        }
    }
}