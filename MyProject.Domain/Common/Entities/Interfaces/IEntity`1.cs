namespace MyProject.Domain.Entities.Interfaces;

public interface IEntity<TKey> : IEntity
{
    TKey Id { get; }
}