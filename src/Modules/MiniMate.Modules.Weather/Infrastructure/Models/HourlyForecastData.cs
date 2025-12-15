namespace MiniMate.Modules.Weather.Infrastructure.Models;

/// <summary>
/// Represents weather forecast data for a single hour
/// </summary>
public record HourlyForecastData
{
    public DateTime Time { get; init; }
    public double Temperature { get; init; }
    public int PrecipitationProbability { get; init; }
    public double Precipitation { get; init; }
    public double Rain { get; init; }
    public double Showers { get; init; }
    public int WeatherCode { get; init; }
    public int CloudCover { get; init; }
    public double WindSpeed { get; init; }
    public int WindDirection { get; init; }
    public int Humidity { get; init; }
    public bool IsDay { get; init; }

    // Formatted properties with units
    public string TemperatureFormatted => $"{Temperature:F1}°C";
    public string PrecipitationProbabilityFormatted => $"{PrecipitationProbability}%";
    public string PrecipitationFormatted => $"{Precipitation:F1} mm";
    public string RainFormatted => $"{Rain:F1} mm";
    public string HumidityFormatted => $"{Humidity}%";
    public string WindSpeedFormatted => $"{WindSpeed:F1} km/h";
    public string CloudCoverFormatted => $"{CloudCover}%";

    public string TimeFormatted => Time.ToString("HH:mm");
    public string WeatherIcon => GetWeatherIcon(WeatherCode, IsDay);
    public string WeatherDescription => GetWeatherDescription(WeatherCode);

    private static string GetWeatherIcon(int code, bool isDay) => code switch
    {
        0 => isDay ? "☀️" : "🌙",
        1 => isDay ? "🌤️" : "🌙",
        2 => "⛅",
        3 => "☁️",
        45 or 48 => "🌫️",
        51 or 53 or 55 => "🌦️",
        56 or 57 => "🌨️",
        61 or 63 or 65 => "🌧️",
        66 or 67 => "🌨️",
        71 or 73 or 75 or 77 => "❄️",
        80 or 81 or 82 => "🌦️",
        85 or 86 => "🌨️",
        95 or 96 or 99 => "⛈️",
        _ => "❓"
    };

    private static string GetWeatherDescription(int code) => code switch
    {
        0 => "Clear sky",
        1 => "Mainly clear",
        2 => "Partly cloudy",
        3 => "Cloudy",
        45 => "Fog",
        48 => "Depositing rime fog",
        51 => "Light drizzle",
        53 => "Moderate drizzle",
        55 => "Heavy drizzle",
        56 => "Light freezing drizzle",
        57 => "Heavy freezing drizzle",
        61 => "Light rain",
        63 => "Moderate rain",
        65 => "Heavy rain",
        66 => "Light freezing rain",
        67 => "Heavy freezing rain",
        71 => "Light snowfall",
        73 => "Moderate snowfall",
        75 => "Heavy snowfall",
        77 => "Snow grains",
        80 => "Light rain showers",
        81 => "Moderate rain showers",
        82 => "Heavy rain showers",
        85 => "Light snow showers",
        86 => "Heavy snow showers",
        95 => "Thunderstorm",
        96 => "Thunderstorm with light hail",
        99 => "Thunderstorm with heavy hail",
        _ => "Unknown"
    };
}
