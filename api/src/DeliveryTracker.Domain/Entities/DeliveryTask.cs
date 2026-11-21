using DeliveryTracker.Domain.Common;
using DeliveryTracker.Domain.Enums;
using DeliveryTracker.Domain.ValueObjects;

namespace DeliveryTracker.Domain.Entities;

/// <summary>One delivery: who, from where, to where, and how far along it is.</summary>
public class DeliveryTask : Entity
{
    /// <summary>Short code for humans, e.g. "DT-1042".</summary>
    public required string Reference { get; set; }

    // Later (status rules task): the setter becomes private and changes go through a
    // ChangeStatus(...) method that checks the allowed transitions and writes the history.
    public DeliveryTaskStatus Status { get; set; } = DeliveryTaskStatus.Created;

    public required string CustomerName { get; set; }
    public string? CustomerPhone { get; set; }

    public required Location Pickup { get; set; }
    public required Location Dropoff { get; set; }

    public DateTimeOffset? TimeWindowStart { get; set; }
    public DateTimeOffset? TimeWindowEnd { get; set; }

    public string? Notes { get; set; }

    public Guid? DriverId { get; set; }
    public Driver? Driver { get; set; }

    /// <summary>The dispatcher who created it. The foreign key to users is added with Identity.</summary>
    public Guid CreatedByUserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public List<TaskStatusHistory> StatusHistory { get; } = [];
}
