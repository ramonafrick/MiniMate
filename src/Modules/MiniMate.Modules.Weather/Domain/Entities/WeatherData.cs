using MiniMate.Modules.Weather.Domain.ValueObjects;
using MiniMate.Shared.Kernel.Contracts;

namespace MiniMate.Modules.Weather.Domain.Entities;

/// <summary>
/// Core weather data entity containing current weather information and location details.
/// Implements IWeatherData to enable cross-module usage (e.g., Clothing module).
/// Contains rich business logic for weather interpretation, wind direction, icons, etc.
/// </summary>
public record WeatherData(
    double Latitude,
    double Longitude,
    string? Timezone,
    string? TimezoneAbbreviation,
    int UtcOffsetSeconds,
    CurrentWeather Current,
    CurrentUnits? Units
) : IWeatherData
{
    /// <summary>
    /// Returns true if the location is in the Northern Hemisphere
    /// </summary>
    public bool IsNorthernHemisphere => Latitude >= 0;

    public string Temperature => $"{Current.Temperature:F1}{Units?.Temperature ?? "°C"}";
    public string ApparentTemperature => $"{Current.ApparentTemperature:F1}{Units?.ApparentTemperature ?? "°C"}";
    public string Humidity => $"{Current.RelativeHumidity}{Units?.RelativeHumidity ?? "%"}";
    public string Precipitation => $"{Current.Precipitation:F1}{Units?.Precipitation ?? "mm"}";
    public string Rain => $"{Current.Rain:F1}{Units?.Rain ?? "mm"}";
    public string Snowfall => $"{Current.Snowfall:F1}{Units?.Snowfall ?? "cm"}";
    public string WindSpeed => $"{Current.WindSpeed:F1}{Units?.WindSpeed ?? "km/h"}";
    public string WindGusts => $"{Current.WindGusts:F1}{Units?.WindGusts ?? "km/h"}";
    public string Pressure => $"{Current.SurfacePressure:F1}{Units?.SurfacePressure ?? "hPa"}";
    public string CloudCover => $"{Current.CloudCover}%";
    public string UvIndex => $"{Current.UvIndex:F1}";
    public string Visibility => $"{Current.Visibility / 1000:F1} km";
    public bool IsDay => Current.IsDay == 1;

    /// <summary>
    /// Gets the current local time for the weather data location.
    /// Uses the UTC offset provided by the API for accurate timezone calculation.
    /// This approach works reliably across all platforms and handles half-hour offsets (e.g., Adelaide UTC+9:30).
    /// </summary>
    public DateTime LocalTime
    {
        get
        {
            // Calculate local time using UTC offset (more reliable than timezone ID conversion)
            // UtcOffsetSeconds comes directly from the API (e.g., 37800 = 10.5 hours for Adelaide)
            var offset = TimeSpan.FromSeconds(UtcOffsetSeconds);
            return DateTime.UtcNow.Add(offset);
        }
    }

    /// <summary>
    /// Gets the timestamp when the weather data was last updated by the API.
    /// This is the time from Current.Time, converted to the location's timezone.
    /// </summary>
    public DateTime LastUpdateTime
    {
        get
        {
            try
            {
                // Parse the API update time (format: "yyyy-MM-ddTHH:mm")
                var updateTimeUtc = DateTime.ParseExact(Current.Time, "yyyy-MM-ddTHH:mm", null, System.Globalization.DateTimeStyles.AssumeUniversal);

                if (string.IsNullOrEmpty(Timezone))
                    return updateTimeUtc;

                // The API already returns time in the location's timezone when using timezone=auto
                // So we just parse it as local to that timezone
                return DateTime.ParseExact(Current.Time, "yyyy-MM-ddTHH:mm", null);
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }

    public string WindDirection => GetWindDirection(Current.WindDirection);
    public string WeatherDescription => GetWeatherDescription(Current.WeatherCode);
    public string WeatherIcon => GetWeatherIcon(Current.WeatherCode, IsDay);

    // Note: WeatherResources will be handled via IStringLocalizer in the application layer
    // For now, using hardcoded English translations until localization is properly integrated

    private static string GetWindDirection(int degrees)
    {
        // Normalize to 0-359 degrees
        degrees = ((degrees % 360) + 360) % 360;

        return degrees switch
        {
            >= 0 and < 12 => "N",
            >= 12 and < 34 => "NNE",
            >= 34 and < 56 => "NE",
            >= 56 and < 78 => "ENE",
            >= 78 and < 102 => "E",
            >= 102 and < 124 => "ESE",
            >= 124 and < 146 => "SE",
            >= 146 and < 168 => "SSE",
            >= 168 and < 192 => "S",
            >= 192 and < 214 => "SSW",
            >= 214 and < 236 => "SW",
            >= 236 and < 258 => "WSW",
            >= 258 and < 282 => "W",
            >= 282 and < 304 => "WNW",
            >= 304 and < 326 => "NW",
            >= 326 and < 348 => "NNW",
            >= 348 => "N"
        };
    }

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

    // Explicit implementation of IWeatherData.Current
    ICurrentWeather IWeatherData.Current => Current;
}
