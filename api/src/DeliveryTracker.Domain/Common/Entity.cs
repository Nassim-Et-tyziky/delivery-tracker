namespace DeliveryTracker.Domain.Common;

/// <summary>Base class for everything stored with its own id.</summary>
public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.CreateVersion7();
}
