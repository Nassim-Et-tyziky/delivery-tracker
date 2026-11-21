using DeliveryTracker.Domain.Common;
using DeliveryTracker.Domain.Enums;

namespace DeliveryTracker.Domain.Entities;

/// <summary>One status change of a task, for the timeline and reports.</summary>
public class TaskStatusHistory : Entity
{
    public Guid TaskId { get; set; }

    /// <summary>Null for the first entry (the task was created).</summary>
    public DeliveryTaskStatus? FromStatus { get; set; }
    public DeliveryTaskStatus ToStatus { get; set; }

    /// <summary>Required when ToStatus is Failed.</summary>
    public string? Reason { get; set; }

    /// <summary>Who made the change. The foreign key to users is added with Identity.</summary>
    public Guid ChangedByUserId { get; set; }

    public DateTimeOffset ChangedAt { get; set; } = DateTimeOffset.UtcNow;
}
