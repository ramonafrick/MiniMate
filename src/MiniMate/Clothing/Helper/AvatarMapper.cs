using MiniMate.Clothing.Models;

namespace MiniMate.Clothing.Helper
{
    /// <summary>
    /// Maps weather descriptions to corresponding avatar image paths
    /// </summary>
    public static class AvatarMapper
    {
        /// <summary>
        /// Gets the avatar image path for a given weather description
        /// </summary>
        /// <param name="description">The weather description</param>
        /// <returns>Path to the corresponding avatar image</returns>
        public static string GetAvatarPath(WeatherDescription description)
        {
            return description switch
            {
                // Extreme cold conditions
                WeatherDescription.ExtremeColdWithSnow => "images/avatars/extreme-winter.jpg",
                WeatherDescription.ExtremeCold => "images/avatars/extreme-cold.jpg",

                // Cold with snow
                WeatherDescription.ColdWithHeavySnow => "images/avatars/snow-day.jpg",
                WeatherDescription.ColdWithSnow => "images/avatars/winter-snow.jpg",
                WeatherDescription.Cold => "images/avatars/winter.jpg",

                // Cool conditions
                WeatherDescription.CoolWithHeavyRain => "images/avatars/rainy-cold.jpg",
                WeatherDescription.CoolWithRain => "images/avatars/rainy-cool.jpg",
                WeatherDescription.CoolAndWindy => "images/avatars/windy-cool.jpg",
                WeatherDescription.Cool => "images/avatars/cool.jpg",

                // Mild conditions
                WeatherDescription.MildWithRain => "images/avatars/mild-rainy.jpg",
                WeatherDescription.Mild => "images/avatars/mild.jpg",

                // Warm conditions
                WeatherDescription.WarmWithRain => "images/avatars/warm-rainy.jpg",
                WeatherDescription.Warm => "images/avatars/warm.jpg",

                // Hot conditions
                WeatherDescription.HotWithThunderstorm => "images/avatars/thunderstorm.jpg",
                WeatherDescription.HotWithRain => "images/avatars/hot-rainy.jpg",
                WeatherDescription.VeryHot => "images/avatars/very-hot.jpg",
                WeatherDescription.Hot => "images/avatars/hot.jpg",

                // Default fallback
                _ => "images/avatars/default.jpg"
            };
        }
    }
}
