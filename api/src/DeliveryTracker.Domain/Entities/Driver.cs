using DeliveryTracker.Domain.Common;

namespace DeliveryTracker.Domain.Entities;

/// <summary>Extra information for a user with the Driver role.</summary>
public class Driver : Entity
{
    /// <summary>The login account. The foreign key to users is added with ASP.NET Core Identity.</summary>
    public Guid UserId { get; set; }

    public string? VehicleType { get; set; }

    /// <summary>The online / offline switch in the driver app.</summary>
    public bool IsAvailable { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public List<DeliveryTask> Tasks { get; } = [];
}
