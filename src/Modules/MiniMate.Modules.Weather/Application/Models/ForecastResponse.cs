using System.Text.Json.Serialization;

namespace MiniMate.Modules.Weather.Application.Models
{
    public record ForecastResponse(
        [property: JsonPropertyName("latitude")] double Latitude,
        [property: JsonPropertyName("longitude")] double Longitude,
        [property: JsonPropertyName("hourly")] HourlyForecast? Hourly,
        [property: JsonPropertyName("hourly_units")] HourlyUnits? HourlyUnits
    );
}
