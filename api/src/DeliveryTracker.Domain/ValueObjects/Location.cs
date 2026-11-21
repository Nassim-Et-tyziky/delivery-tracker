namespace DeliveryTracker.Domain.ValueObjects;

/// <summary>
/// An address and its optional map position. A value object: it has no id and is compared by its values.
/// Stored as columns on the owning table (pickup_address, pickup_latitude, ...).
/// </summary>
public sealed record Location(string Address, double? Latitude, double? Longitude);
