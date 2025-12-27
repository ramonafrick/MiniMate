using System.Text.Json.Serialization;

namespace MiniMate.Modules.Weather.Application.Models
{
    public record DailyUnits(
        [property: JsonPropertyName("time")] string? Time,
        [property: JsonPropertyName("temperature_2m_max")] string? TemperatureMax,
        [property: JsonPropertyName("temperature_2m_min")] string? TemperatureMin,
        [property: JsonPropertyName("precipitation_probability_max")] string? PrecipitationProbabilityMax,
        [property: JsonPropertyName("precipitation_sum")] string? PrecipitationSum,
        [property: JsonPropertyName("rain_sum")] string? RainSum,
        [property: JsonPropertyName("weather_code")] string? WeatherCode,
        [property: JsonPropertyName("wind_speed_10m_max")] string? WindSpeedMax,
        [property: JsonPropertyName("wind_gusts_10m_max")] string? WindGustsMax
    );
}
