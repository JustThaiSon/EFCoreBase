using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MyProject.Domain.Entities;

namespace MyProject.Infrastructure.Persistence.HandleContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext([NotNull] DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    public ApplicationDbContext()
    {

    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.LogTo(Console.WriteLine);
        optionsBuilder.UseSqlServer("Server=DESKTOP-ADMIN;Database=StreamieDB;Trusted_Connection=True;TrustServerCertificate=True;");
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