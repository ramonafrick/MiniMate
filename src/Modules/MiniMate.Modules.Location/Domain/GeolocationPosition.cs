namespace MiniMate.Modules.Location.Domain
{
    public class GeolocationPosition
    {
        public GeolocationCoordinates Coords { get; set; } = new();
        public long Timestamp { get; set; }
    }
}
