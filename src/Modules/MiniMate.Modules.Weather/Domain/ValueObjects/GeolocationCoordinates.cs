namespace MiniMate.Modules.Weather.Domain.ValueObjects;

/// <summary>
/// Represents geographic coordinates (latitude, longitude) with optional accuracy.
/// </summary>
public class GeolocationCoordinates
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Accuracy { get; set; }
}
