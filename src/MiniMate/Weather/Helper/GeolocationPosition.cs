namespace MiniMate.Weather.Helper
{
    public class GeolocationPosition
    {
        public GeolocationCoordinates Coords { get; set; } = new();
        public long Timestamp { get; set; }
    }
}
