using JwtAuth.Data;
using JwtAuth.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuth.Repositories
{
    public class UserSessionRepository
    {
        private readonly AppDbContext _context;
        public UserSessionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserSession>?> GetUserSessionsAsync(int userId)
        {
            return await _context.UserSessions
                .Include(us => us.User)
                .Where(us => us.UserId == userId)
                .ToListAsync();
        }
        public async Task<UserSession> AddUserSessionAsync(UserSession userSession)
        {
            await _context.UserSessions.AddAsync(userSession);
            await _context.SaveChangesAsync();

            return userSession;
        }
        public async Task<UserSession> UpdateUserSessionAsync(UserSession userSession)
        {
            _context.UserSessions.Update(userSession);
            await _context.SaveChangesAsync();

            return userSession;
        }

        public async Task<UserSession?> GetUserSessionByRefreshTokenAsync(int userId, string refreshToken)
        {
            return await _context.UserSessions
                .Include(us => us.User)
                .FirstOrDefaultAsync(us => us.RefreshToken == refreshToken && !us.IsRevoked && us.UserId == userId);
        }

        public async Task<bool> RevokeUserSessionAsync(int userId, string refreshToken)
        {
            var session = await GetUserSessionByRefreshTokenAsync(userId, refreshToken);
            if (session != null)
            {
                session.IsRevoked = true;
                _context.UserSessions.Update(session);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RevokeAllUserSessionsAsync(int userId)
        {
            var sessions = await GetUserSessionsAsync(userId);
            if (sessions != null)
            {
                foreach (var session in sessions)
                {
                    session.IsRevoked = true;
                }
                _context.UserSessions.UpdateRange(sessions);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        //public async Task DeleteUserSessionAsync(string userId)
        //{
        //    var session = await GetUserSessionAsync(userId);
        //    if (session != null)
        //    {
        //        _context.UserSessions.Remove(session);
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}
