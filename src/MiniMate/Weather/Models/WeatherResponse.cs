using System.Text.Json.Serialization;

namespace MiniMate.Weather.Models
{
    public record WeatherResponse(
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("current")] CurrentWeather? Current,
    [property: JsonPropertyName("current_units")] CurrentUnits? CurrentUnits
);
}
