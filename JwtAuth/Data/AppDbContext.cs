using JwtAuth.Data.Interceptors;
using JwtAuth.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuth.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new AuditableInterceptor());
        }
    }
}
