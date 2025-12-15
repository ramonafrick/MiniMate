using System.Text.Json.Serialization;

namespace MiniMate.Modules.Weather.Infrastructure.Models;

public record ForecastResponse(
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("hourly")] HourlyForecast? Hourly,
    [property: JsonPropertyName("hourly_units")] HourlyUnits? HourlyUnits
);

public record HourlyUnits(
    [property: JsonPropertyName("time")] string? Time,
    [property: JsonPropertyName("temperature_2m")] string? Temperature,
    [property: JsonPropertyName("precipitation_probability")] string? PrecipitationProbability,
    [property: JsonPropertyName("precipitation")] string? Precipitation,
    [property: JsonPropertyName("rain")] string? Rain,
    [property: JsonPropertyName("showers")] string? Showers,
    [property: JsonPropertyName("weather_code")] string? WeatherCode,
    [property: JsonPropertyName("cloud_cover")] string? CloudCover,
    [property: JsonPropertyName("wind_speed_10m")] string? WindSpeed,
    [property: JsonPropertyName("wind_direction_10m")] string? WindDirection,
    [property: JsonPropertyName("relative_humidity_2m")] string? Humidity
);
