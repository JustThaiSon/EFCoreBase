using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MyProject.Domain.Entities;

namespace MyProject.Infrastructure.Persistence.HandleContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }
    public ApplicationDbContext([NotNull] DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.LogTo(Console.WriteLine);
        optionsBuilder.UseNpgsql("Host=ballast.proxy.rlwy.net;Port=48465;Database=railway;Username=postgres;Password=WtbBZJwVKRHsfBEbVjjEaYugAEBqYbHN;SSL Mode=Require;Trust Server Certificate=true");
    }

    public DbSet<NguyenEntity> NguyenEntities { get; set; }
    public DbSet<Testtiep> Testtieps { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplySoftDeleteGlobalFilter();
        modelBuilder.ApplyAuditPrecision();

        modelBuilder.Entity<NguyenEntity>().ToTable("NGUYEN");
        modelBuilder.Entity<NguyenEntity>(entity => { entity.HasKey(c => c.Id); });

        modelBuilder.Entity<Testtiep>().ToTable("EntityTest");
        modelBuilder.Entity<Testtiep>(entity => { entity.HasKey(c => c.Id); });
    }
}