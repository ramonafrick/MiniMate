namespace MiniMate.Modules.Weather.Domain.ValueObjects;

/// <summary>
/// Represents a geographic position obtained from browser geolocation API.
/// </summary>
public class GeolocationPosition
{
    public GeolocationCoordinates Coords { get; set; } = new();
    public long Timestamp { get; set; }
}
