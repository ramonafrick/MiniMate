using System.Globalization;

namespace MiniMate.Modules.Weather.Infrastructure.Models;

/// <summary>
/// Represents weather forecast data for a single day
/// </summary>
public record DailyForecastData
{
    public DateTime Date { get; init; }
    public double TemperatureMax { get; init; }
    public double TemperatureMin { get; init; }
    public int PrecipitationProbability { get; init; }
    public double PrecipitationSum { get; init; }
    public double RainSum { get; init; }
    public int WeatherCode { get; init; }
    public DateTime? Sunrise { get; init; }
    public DateTime? Sunset { get; init; }
    public double WindSpeedMax { get; init; }
    public double WindGustsMax { get; init; }

    // Formatted properties
    public string TemperatureMaxFormatted => $"{TemperatureMax:F1}°C";
    public string TemperatureMinFormatted => $"{TemperatureMin:F1}°C";
    public string TemperatureRangeFormatted => $"{TemperatureMin:F1}° / {TemperatureMax:F1}°";
    public string PrecipitationProbabilityFormatted => $"{PrecipitationProbability}%";
    public string PrecipitationSumFormatted => $"{PrecipitationSum:F1} mm";
    public string RainSumFormatted => $"{RainSum:F1} mm";
    public string WindSpeedMaxFormatted => $"{WindSpeedMax:F1} km/h";

    public string DayName
    {
        get
        {
            var culture = CultureInfo.CurrentUICulture;
            var today = DateTime.Today;

            if (Date.Date == today)
                return culture.TwoLetterISOLanguageName == "de" ? "Heute" : "Today";
            else if (Date.Date == today.AddDays(1))
                return culture.TwoLetterISOLanguageName == "de" ? "Morgen" : "Tomorrow";
            else
                return Date.ToString("dddd", culture);
        }
    }

    public string DateFormatted => Date.ToString("dd.MM", CultureInfo.CurrentUICulture);
    public string WeatherIcon => GetWeatherIcon(WeatherCode);
    public string WeatherDescription => GetWeatherDescription(WeatherCode);

    private static string GetWeatherIcon(int code) => code switch
    {
        0 => "☀️",
        1 => "🌤️",
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
