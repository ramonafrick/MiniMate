using System.Text.Json.Serialization;

namespace MiniMate.Modules.Weather.Infrastructure.Models;

public record DailyForecastResponse(
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("daily")] DailyForecast? Daily,
    [property: JsonPropertyName("daily_units")] DailyUnits? DailyUnits
);

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
