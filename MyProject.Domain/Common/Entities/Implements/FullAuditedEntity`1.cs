using MyProject.Domain.Entities.Interfaces.Audited;
using MyProject.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Domain.Entities.Implements;

public class FullAuditedEntity<TKey> :
    FullAuditedEntity,
    IAuditedEntity<TKey>,
    ICreationAuditedEntity,
    IModificationAuditedEntity,
    IDeletionAuditedEntity,
    IEntity<TKey>,
    IEntity
{
    [Key]
    public required TKey Id { get; set; }
}