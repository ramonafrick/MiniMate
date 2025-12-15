using System.Text.Json.Serialization;
using MiniMate.Shared.Kernel.Contracts;

namespace MiniMate.Modules.Weather.Domain.ValueObjects;

/// <summary>
/// Represents current weather conditions as provided by the weather API.
/// Implements ICurrentWeather to enable cross-module usage (e.g., Clothing module).
/// </summary>
public record CurrentWeather(
    [property: JsonPropertyName("time")] string Time,
    [property: JsonPropertyName("interval")] int Interval,
    [property: JsonPropertyName("temperature_2m")] double Temperature,
    [property: JsonPropertyName("relative_humidity_2m")] int RelativeHumidity,
    [property: JsonPropertyName("apparent_temperature")] double ApparentTemperature,
    [property: JsonPropertyName("is_day")] int IsDay,
    [property: JsonPropertyName("precipitation")] double Precipitation,
    [property: JsonPropertyName("rain")] double Rain,
    [property: JsonPropertyName("showers")] double Showers,
    [property: JsonPropertyName("snowfall")] double Snowfall,
    [property: JsonPropertyName("weather_code")] int WeatherCode,
    [property: JsonPropertyName("cloud_cover")] int CloudCover,
    [property: JsonPropertyName("surface_pressure")] double SurfacePressure,
    [property: JsonPropertyName("wind_speed_10m")] double WindSpeed,
    [property: JsonPropertyName("wind_direction_10m")] int WindDirection,
    [property: JsonPropertyName("wind_gusts_10m")] double WindGusts,
    [property: JsonPropertyName("uv_index")] double UvIndex,
    [property: JsonPropertyName("visibility")] double Visibility
) : ICurrentWeather;
