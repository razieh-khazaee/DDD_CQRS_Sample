using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Shared.Audit;

public class FillAuditFieldsInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetShadowProperties(eventData);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetShadowProperties(eventData);

        return base.SavingChanges(eventData, result);
    }

    private static void SetShadowProperties(DbContextEventData eventData)
    {
        var changeTracker = eventData.Context.ChangeTracker;
        DateTime now = DateTime.Now;

        var auditableEntities = changeTracker.Entries().Where(m => m.Entity is IAuditable).ToList();
        foreach (var entry in auditableEntities)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(AuditConstants.CreatedBy).CurrentValue = "";
                entry.Property(AuditConstants.CreatedDate).CurrentValue = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(AuditConstants.UpdatedBy).CurrentValue = "";
                entry.Property(AuditConstants.UpdatedDate).CurrentValue = now;
            }
        }
    }
}