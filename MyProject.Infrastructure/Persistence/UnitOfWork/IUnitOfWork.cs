using Microsoft.EntityFrameworkCore;

namespace MyProject.Infrastructure;

public interface IUnitOfWork<TDbContext> : IDisposable where TDbContext : DbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
    Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);
}
