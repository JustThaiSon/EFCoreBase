namespace MyProject.Domain.Entities.Interfaces.Audited;

public interface ICreationAuditedEntity
{
    DateTimeOffset CreatedTime { get; set; }
    Guid? CreatedBy { get; set; }
}