namespace DeliveryTracker.Domain.Enums;

/// <summary>
/// Lifecycle of a delivery task. Sent in JSON as strings ("InTransit"), see contracts/openapi.yaml.
/// Named DeliveryTaskStatus (not TaskStatus) to avoid a clash with System.Threading.Tasks.TaskStatus.
/// </summary>
public enum DeliveryTaskStatus
{
    Created,
    Assigned,
    Accepted,
    PickedUp,
    InTransit,
    Delivered,
    Failed,
    Cancelled,
}
