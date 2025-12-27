using MiniMate.Modules.Weather.Resources;
using System.Globalization;

namespace MiniMate.Modules.Weather.Domain
{
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
    }
}
