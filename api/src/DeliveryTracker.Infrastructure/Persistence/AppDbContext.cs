using DeliveryTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryTracker.Infrastructure.Persistence;

/// <summary>EF Core's "unit of work": one DbSet per table, and the mapping between classes and tables.</summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<DeliveryTask> DeliveryTasks => Set<DeliveryTask>();
    public DbSet<TaskStatusHistory> TaskStatusHistory => Set<TaskStatusHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Picks up every IEntityTypeConfiguration<T> class in this project (Configurations/ folder).
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
