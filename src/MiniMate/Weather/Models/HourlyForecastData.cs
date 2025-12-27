using MiniMate.Weather.Resources;

namespace MiniMate.Weather.Models
{
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
