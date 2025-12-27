using System.Text.Json.Serialization;

namespace MiniMate.Modules.Weather.Application.Models
{
    public record HourlyForecast(
        [property: JsonPropertyName("time")] string[] Time,
        [property: JsonPropertyName("temperature_2m")] double[] Temperature,
        [property: JsonPropertyName("precipitation_probability")] int[] PrecipitationProbability,
        [property: JsonPropertyName("precipitation")] double[] Precipitation,
        [property: JsonPropertyName("rain")] double[] Rain,
        [property: JsonPropertyName("showers")] double[] Showers,
        [property: JsonPropertyName("weather_code")] int[] WeatherCode,
        [property: JsonPropertyName("cloud_cover")] int[] CloudCover,
        [property: JsonPropertyName("wind_speed_10m")] double[] WindSpeed,
        [property: JsonPropertyName("wind_direction_10m")] int[] WindDirection,
        [property: JsonPropertyName("relative_humidity_2m")] int[] Humidity,
        [property: JsonPropertyName("is_day")] int[] IsDay
    );
}
