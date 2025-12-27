using System.Text.Json.Serialization;

namespace MiniMate.Modules.Weather.Application.Models
{
    public record DailyForecastResponse(
        [property: JsonPropertyName("latitude")] double Latitude,
        [property: JsonPropertyName("longitude")] double Longitude,
        [property: JsonPropertyName("daily")] DailyForecast? Daily,
        [property: JsonPropertyName("daily_units")] DailyUnits? DailyUnits
    );
}
