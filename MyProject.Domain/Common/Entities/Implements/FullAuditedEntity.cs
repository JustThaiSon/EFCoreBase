using MyProject.Domain.Entities.Interfaces.Audited;
using MyProject.Domain.Entities.Interfaces;

namespace MyProject.Domain.Entities.Implements;

public class FullAuditedEntity :
    Entity,
    IAuditedEntity,
    ICreationAuditedEntity,
    IModificationAuditedEntity,
    IDeletionAuditedEntity,
    IEntity
{
    public DateTimeOffset CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
    public Guid? DeletedBy { get; set; }
    public DateTimeOffset DeletedTime { get; set; }
}