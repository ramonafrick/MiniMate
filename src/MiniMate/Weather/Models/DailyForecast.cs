using System.Text.Json.Serialization;

namespace MiniMate.Weather.Models
{
    public record DailyForecast(
        [property: JsonPropertyName("time")] string[] Time,
        [property: JsonPropertyName("temperature_2m_max")] double[] TemperatureMax,
        [property: JsonPropertyName("temperature_2m_min")] double[] TemperatureMin,
        [property: JsonPropertyName("precipitation_probability_max")] int[] PrecipitationProbabilityMax,
        [property: JsonPropertyName("precipitation_sum")] double[] PrecipitationSum,
        [property: JsonPropertyName("rain_sum")] double[] RainSum,
        [property: JsonPropertyName("weather_code")] int[] WeatherCode,
        [property: JsonPropertyName("sunrise")] string[] Sunrise,
        [property: JsonPropertyName("sunset")] string[] Sunset,
        [property: JsonPropertyName("wind_speed_10m_max")] double[] WindSpeedMax,
        [property: JsonPropertyName("wind_gusts_10m_max")] double[] WindGustsMax
    );
}
