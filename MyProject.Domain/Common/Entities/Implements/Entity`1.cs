using System.ComponentModel.DataAnnotations;
using MyProject.Domain.Entities.Interfaces;

namespace MyProject.Domain.Entities.Implements;

public abstract class Entity<TKey> : Entity, IEntity<TKey>, IEntity
{
    [Key]
    public required TKey Id { get; set; }
}