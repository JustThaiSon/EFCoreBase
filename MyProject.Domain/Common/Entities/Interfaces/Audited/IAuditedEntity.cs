namespace MyProject.Domain.Entities.Interfaces.Audited;

public interface IAuditedEntity :
ICreationAuditedEntity,
IModificationAuditedEntity,
IDeletionAuditedEntity,
IEntity
{

}