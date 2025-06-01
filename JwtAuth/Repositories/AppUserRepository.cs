using JwtAuth.Data;
using JwtAuth.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuth.Repositories
{
    public class AppUserRepository
    {

        private readonly AppDbContext _context;

        public AppUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetUserByIdAsync(int userId)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<AppUser> CreateUserAsync(AppUser user)
        {
            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<AppUser> UpdateUserAsync(AppUser user)
        {
            _context.AppUsers.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                _context.AppUsers.Remove(user);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

    }
}
