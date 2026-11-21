using DeliveryTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryTracker.Infrastructure.Persistence.Configurations;

internal sealed class DeliveryTaskConfiguration : IEntityTypeConfiguration<DeliveryTask>
{
    public void Configure(EntityTypeBuilder<DeliveryTask> builder)
    {
        builder.ToTable("delivery_tasks");

        builder.Property(t => t.Reference).HasMaxLength(20);
        builder.HasIndex(t => t.Reference).IsUnique();

        // Store the enum as text ("InTransit") instead of a number: readable in pgAdmin,
        // and adding a status later never shifts existing values.
        builder.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);

        builder.Property(t => t.CustomerName).HasMaxLength(200);
        builder.Property(t => t.CustomerPhone).HasMaxLength(20);
        builder.Property(t => t.Notes).HasMaxLength(2000);

        // Location is a complex type: no table of its own, its fields become columns here.
        builder.ComplexProperty(t => t.Pickup, l =>
        {
            l.Property(p => p.Address).HasColumnName("pickup_address").HasMaxLength(500);
            l.Property(p => p.Latitude).HasColumnName("pickup_latitude");
            l.Property(p => p.Longitude).HasColumnName("pickup_longitude");
        });
        builder.ComplexProperty(t => t.Dropoff, l =>
        {
            l.Property(p => p.Address).HasColumnName("dropoff_address").HasMaxLength(500);
            l.Property(p => p.Latitude).HasColumnName("dropoff_latitude");
            l.Property(p => p.Longitude).HasColumnName("dropoff_longitude");
        });

        // A task may have no driver yet. Deleting a driver must not delete their tasks.
        builder.HasOne(t => t.Driver)
            .WithMany(d => d.Tasks)
            .HasForeignKey(t => t.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.StatusHistory)
            .WithOne()
            .HasForeignKey(h => h.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        // Speeds up GET /me/tasks (a driver's open tasks) and the dashboard's status filter.
        builder.HasIndex(t => new { t.DriverId, t.Status });
        builder.HasIndex(t => t.Status);
    }
}
