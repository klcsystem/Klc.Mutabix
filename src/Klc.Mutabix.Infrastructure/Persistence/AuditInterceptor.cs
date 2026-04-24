using System.Text.Json;
using Klc.Mutabix.Domain.Common;
using Klc.Mutabix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Klc.Mutabix.Infrastructure.Persistence;

public class AuditInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not MutabixDbContext context)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .Where(e => e.Entity is not AuditLog)
            .ToList();

        foreach (var entry in entries)
        {
            var auditLog = new AuditLog
            {
                EntityName = entry.Entity.GetType().Name,
                EntityId = entry.Entity.Id,
                Action = entry.State.ToString()
            };

            if (entry.State == EntityState.Modified)
            {
                var oldValues = new Dictionary<string, object?>();
                var newValues = new Dictionary<string, object?>();

                foreach (var prop in entry.Properties.Where(p => p.IsModified))
                {
                    oldValues[prop.Metadata.Name] = prop.OriginalValue;
                    newValues[prop.Metadata.Name] = prop.CurrentValue;
                }

                auditLog.OldValues = JsonSerializer.Serialize(oldValues);
                auditLog.NewValues = JsonSerializer.Serialize(newValues);
            }
            else if (entry.State == EntityState.Added)
            {
                var newValues = entry.Properties
                    .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);
                auditLog.NewValues = JsonSerializer.Serialize(newValues);
            }

            context.AuditLogs.Add(auditLog);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
