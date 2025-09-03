using Microsoft.EntityFrameworkCore;
using MyProject.Domain.Entities.Interfaces;

namespace MyProject.Infrastructure.Repository;

public interface IRepository<TDbContext, TEntity> : IDisposable
    where TEntity : class, IEntity
    where TDbContext : DbContext
{
    DbSet<TEntity> Entities { get; }

    TEntity Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);

    TEntity Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    TEntity Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);

    IQueryable<TEntity> AsQueryable();
    IQueryable<TEntity> AsNoTrackingQueryable();

    int SaveChanges();
}