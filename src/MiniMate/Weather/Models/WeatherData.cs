using MiniMate.Weather.Resources;

namespace MiniMate.Weather.Models
{
    public record WeatherData(double Latitude, double Longitude, string? Timezone, CurrentWeather Current, CurrentUnits? Units)
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
        /// Converts the current UTC time to the location's timezone.
        /// </summary>
        public DateTime LocalTime
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(Timezone))
                        return DateTime.Now;

                    var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Timezone);
                    return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
                }
                catch
                {
                    // Fallback to system local time if timezone conversion fails
                    return DateTime.Now;
                }
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

        private static string GetWindDirection(int degrees)
        {
            // Normalisiere den Wert auf 0-359 Grad
            degrees = ((degrees % 360) + 360) % 360;

            return degrees switch
            {
                >= 0 and < 12 => WeatherResources.Wind_N,
                >= 12 and < 34 => WeatherResources.Wind_NNE,
                >= 34 and < 56 => WeatherResources.Wind_NE,
                >= 56 and < 78 => WeatherResources.Wind_ENE,
                >= 78 and < 102 => WeatherResources.Wind_E,
                >= 102 and < 124 => WeatherResources.Wind_ESE,
                >= 124 and < 146 => WeatherResources.Wind_SE,
                >= 146 and < 168 => WeatherResources.Wind_SSE,
                >= 168 and < 192 => WeatherResources.Wind_S,
                >= 192 and < 214 => WeatherResources.Wind_SSW,
                >= 214 and < 236 => WeatherResources.Wind_SW,
                >= 236 and < 258 => WeatherResources.Wind_WSW,
                >= 258 and < 282 => WeatherResources.Wind_W,
                >= 282 and < 304 => WeatherResources.Wind_WNW,
                >= 304 and < 326 => WeatherResources.Wind_NW,
                >= 326 and < 348 => WeatherResources.Wind_NNW,
                >= 348 => WeatherResources.Wind_N // Explizit für 348-359
            };
        }

        private static string GetWeatherDescription(int code) => code switch
        {
            0 => WeatherResources.Weather_Clear,
            1 => WeatherResources.Weather_MainlyClear,
            2 => WeatherResources.Weather_PartlyCloudy,
            3 => WeatherResources.Weather_Cloudy,
            45 => WeatherResources.Weather_Fog,
            48 => WeatherResources.Weather_RimeFog,
            51 => WeatherResources.Weather_LightDrizzle,
            53 => WeatherResources.Weather_ModerateDrizzle,
            55 => WeatherResources.Weather_HeavyDrizzle,
            56 => WeatherResources.Weather_LightFreezingDrizzle,
            57 => WeatherResources.Weather_HeavyFreezingDrizzle,
            61 => WeatherResources.Weather_LightRain,
            63 => WeatherResources.Weather_ModerateRain,
            65 => WeatherResources.Weather_HeavyRain,
            66 => WeatherResources.Weather_LightFreezingRain,
            67 => WeatherResources.Weather_HeavyFreezingRain,
            71 => WeatherResources.Weather_LightSnowfall,
            73 => WeatherResources.Weather_ModerateSnowfall,
            75 => WeatherResources.Weather_HeavySnowfall,
            77 => WeatherResources.Weather_SnowGrains,
            80 => WeatherResources.Weather_LightRainShowers,
            81 => WeatherResources.Weather_ModerateRainShowers,
            82 => WeatherResources.Weather_HeavyRainShowers,
            85 => WeatherResources.Weather_LightSnowShowers,
            86 => WeatherResources.Weather_HeavySnowShowers,
            95 => WeatherResources.Weather_Thunderstorm,
            96 => WeatherResources.Weather_ThunderstormLightHail,
            99 => WeatherResources.Weather_ThunderstormHeavyHail,
            _ => WeatherResources.Weather_Unknown
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
    }
}
