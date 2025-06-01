using JwtAuth.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JwtAuth.Data.Interceptors
{
    public class AuditableInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            SetAuditableProperties(eventData);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            SetAuditableProperties(eventData);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void SetAuditableProperties(DbContextEventData eventData)
        {
            var context = eventData.Context;
            if (context == null) return;

            var entries = context.ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditable && (e.State == EntityState.Added || e.State == EntityState.Modified));

            var currentTime = DateTime.Now;

            foreach (var entry in entries)
            {
                var auditableEntity = (IAuditable)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    auditableEntity.CreatedAt = currentTime;
                }
                auditableEntity.UpdatedAt = currentTime;
            }
        }
    }

}
