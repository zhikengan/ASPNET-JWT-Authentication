using JwtAuth.Models;
using JwtAuth.Repositories;

namespace JwtAuth.Services
{
    public class UserService
    {
        private readonly AppUserRepository _userRepo;
        public UserService(AppUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task<AppUser?> GetUserByIdAsync(int userId)
        {
            return await _userRepo.GetUserByIdAsync(userId);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _userRepo.DeleteUserAsync(userId);
        }

        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            return await _userRepo.GetUserByEmailAsync(email);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await _userRepo.GetUserByUsernameAsync(username);
        }


    }
}
