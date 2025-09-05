using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyProject.Domain.Entities;
using MyProject.Domain.Entities.Interfaces.Audited;
using Newtonsoft.Json;

namespace MyProject.Infrastructure;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly Func<Guid?> _getUserId;

    public AuditSaveChangesInterceptor(Func<Guid?> getUserId)
    {
        _getUserId = getUserId;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null) return base.SavingChanges(eventData, result);
        ApplyAudit(eventData.Context);
        AddAuditLogs(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            ApplyAudit(eventData.Context);
            AddAuditLogs(eventData.Context);
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAudit(DbContext context)
    {
        var userId = _getUserId();
        var now = DateTimeOffset.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is ICreationAuditedEntity created && entry.State == EntityState.Added)
            {
                created.CreatedTime = now;
                created.CreatedBy = userId;
            }

            if (entry.Entity is IModificationAuditedEntity modified && (entry.State == EntityState.Modified))
            {
                modified.ModifiedTime = now;
                modified.ModifiedBy = userId;
            }

            if (entry.Entity is IDeletionAuditedEntity deleted && entry.State == EntityState.Deleted)
            {
                // Convert hard delete to soft delete
                entry.State = EntityState.Modified;
                deleted.IsDeleted = true;
                deleted.DeletedTime = now;
                deleted.DeletedBy = userId;
            }
        }
    }
    private void AddAuditLogs(DbContext context)
    {
        var userId = _getUserId();
        var auditEntries = new List<AuditLog>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var audit = new AuditLog
            {
                TableName = entry.Metadata.GetTableName() ?? entry.Entity.GetType().Name,
                ActionType = entry.State.ToString(),
                UserId = userId,
                TimeStamp = DateTimeOffset.UtcNow,
                KeyValues = JsonConvert.SerializeObject(entry.Properties
                    .Where(p => p.Metadata.IsPrimaryKey())
                    .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue))
            };

            if (entry.State == EntityState.Modified)
            {
                audit.OldValues = JsonConvert.SerializeObject(entry.Properties
                    .Where(p => p.IsModified)
                    .ToDictionary(p => p.Metadata.Name, p => p.OriginalValue));

                audit.NewValues = JsonConvert.SerializeObject(entry.Properties
                    .Where(p => p.IsModified)
                    .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue));
            }
            else if (entry.State == EntityState.Added)
            {
                audit.NewValues = JsonConvert.SerializeObject(entry.Properties
                    .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue));
            }
            else if (entry.State == EntityState.Deleted)
            {
                audit.OldValues = JsonConvert.SerializeObject(entry.Properties
                    .ToDictionary(p => p.Metadata.Name, p => p.OriginalValue));
            }

            auditEntries.Add(audit);
        }

        if (auditEntries.Any())
        {
            context.Set<AuditLog>().AddRange(auditEntries);
        }
    }
}
