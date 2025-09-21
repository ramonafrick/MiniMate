using System.Text.Json.Serialization;

namespace MiniMate.Weather.Models
{
    public record WeatherResponse(
    [property: JsonPropertyName("current")] CurrentWeather? Current,
    [property: JsonPropertyName("current_units")] CurrentUnits? CurrentUnits
);
}
