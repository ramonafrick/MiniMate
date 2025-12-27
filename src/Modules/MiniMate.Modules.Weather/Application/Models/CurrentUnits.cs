using System.Text.Json.Serialization;

namespace MiniMate.Modules.Weather.Application.Models
{
    public record CurrentUnits(
    [property: JsonPropertyName("temperature_2m")] string Temperature,
    [property: JsonPropertyName("relative_humidity_2m")] string RelativeHumidity,
    [property: JsonPropertyName("apparent_temperature")] string ApparentTemperature,
    [property: JsonPropertyName("precipitation")] string Precipitation,
    [property: JsonPropertyName("rain")] string Rain,
    [property: JsonPropertyName("showers")] string Showers,
    [property: JsonPropertyName("snowfall")] string Snowfall,
    [property: JsonPropertyName("surface_pressure")] string SurfacePressure,
    [property: JsonPropertyName("wind_speed_10m")] string WindSpeed,
    [property: JsonPropertyName("wind_direction_10m")] string WindDirection,
    [property: JsonPropertyName("wind_gusts_10m")] string WindGusts,
    [property: JsonPropertyName("visibility")] string Visibility
);
}
