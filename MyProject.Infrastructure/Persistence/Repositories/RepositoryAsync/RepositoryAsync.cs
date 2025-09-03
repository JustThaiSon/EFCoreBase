using Microsoft.EntityFrameworkCore;
using MyProject.Domain.Entities.Interfaces;

namespace MyProject.Infrastructure;

public class RepositoryAsync<TEntity> : IRepositoryAsync<TEntity>, IDisposable where TEntity : class, IEntity
{
    private readonly DbContext _primaryDbContext;
    private readonly DbContext _readOnlyDbContext;

    public DbSet<TEntity> Entities { get; }

    public RepositoryAsync(DbContext primaryDbContext, DbContext readOnlyDbContext)
    {
        _primaryDbContext = primaryDbContext ?? throw new ArgumentNullException(nameof(primaryDbContext));
        _readOnlyDbContext = readOnlyDbContext ?? throw new ArgumentNullException(nameof(readOnlyDbContext));
        Entities = _primaryDbContext.Set<TEntity>();
    }

    public RepositoryAsync(DbContext dbContext) : this(dbContext, dbContext) { }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => (await Entities.AddAsync(entity, cancellationToken)).Entity;

    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        => Entities.AddRangeAsync(entities, cancellationToken);

    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        => Task.FromResult(Entities.Update(entity).Entity);

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        Entities.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task<TEntity> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
        => Task.FromResult(Entities.Remove(entity).Entity);

    public Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        Entities.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public virtual IQueryable<TEntity> AsQueryable() => (_readOnlyDbContext ?? _primaryDbContext).Set<TEntity>();

    public virtual IQueryable<TEntity> AsNoTrackingQueryable() => AsQueryable().AsNoTracking();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _primaryDbContext.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        _primaryDbContext?.Dispose();
        if (!ReferenceEquals(_primaryDbContext, _readOnlyDbContext))
            _readOnlyDbContext?.Dispose();
        GC.SuppressFinalize(this);
    }
}
