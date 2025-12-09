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
                // Group 1: Extreme cold winter (< -10°C)
                // WinterJacket, Hat, Scarf, Gloves, WinterBoots/WarmShoes, ThermalUnderwear
                WeatherDescription.ExtremeColdWithSnow => "images/avatars/extreme-cold-winter.jpg",
                WeatherDescription.ExtremeCold => "images/avatars/extreme-cold-winter.jpg",

                // Group 2: Cold weather with snow (< 0°C with snow)
                // WinterJacket, Hat, Gloves, Scarf, WinterBoots/WarmShoes
                WeatherDescription.ColdWithHeavySnow => "images/avatars/cold-snow-weather.jpg",
                WeatherDescription.ColdWithSnow => "images/avatars/cold-snow-weather.jpg",

                // Group 3: Cold clear weather (< 0°C no snow)
                // WinterJacket, Hat, Scarf, WarmShoes
                WeatherDescription.Cold => "images/avatars/cold-clear-weather.jpg",

                // Group 4: Cool rainy weather (0-10°C with rain)
                // RainJacket/Jacket, Umbrella, WaterproofShoes, Sweater
                WeatherDescription.CoolWithHeavyRain => "images/avatars/cool-rainy-weather.jpg",
                WeatherDescription.CoolWithRain => "images/avatars/cool-rainy-weather.jpg",

                // Group 5: Cool windy weather (0-10°C windy)
                // Windbreaker, Sweater, Scarf, Hat, Sneakers
                WeatherDescription.CoolAndWindy => "images/avatars/cool-windy-weather.jpg",

                // Group 6: Cool weather (0-10°C normal)
                // Jacket, Sweater, LongPants, Sneakers, Hat/Scarf (optional)
                WeatherDescription.Cool => "images/avatars/cool-weather.jpg",

                // Group 7: Mild rainy weather (10-15°C with rain)
                // LightJacket, Umbrella, WaterproofShoes
                WeatherDescription.MildWithRain => "images/avatars/mild-rainy-weather.jpg",

                // Group 8: Mild pleasant weather (10-15°C no rain)
                // LightJacket, LongSleeveShirt, Sneakers
                WeatherDescription.Mild => "images/avatars/mild-weather.jpg",

                // Group 9: Warm rainy weather (15-20°C with rain)
                // RainJacket, Umbrella, TShirt, WaterproofShoes
                WeatherDescription.WarmWithRain => "images/avatars/warm-rainy-weather.jpg",

                // Group 10: Warm pleasant weather (15-20°C no rain)
                // LongSleeveShirt, TShirt, Sneakers
                WeatherDescription.Warm => "images/avatars/warm-weather.jpg",

                // Group 11: Hot with thunderstorm (>= 25°C thunderstorm)
                // TShirt, Shorts, WaterproofShoes
                WeatherDescription.HotWithThunderstorm => "images/avatars/hot-thunderstorm.jpg",

                // Group 12: Hot with rain (>= 25°C with rain)
                // TShirt, Shorts, Umbrella, Sandals
                WeatherDescription.HotWithRain => "images/avatars/hot-rainy-weather.jpg",

                // Group 13: Very hot weather (>= 30°C)
                // TShirt, Shorts, SunHat, Sunglasses, Sandals
                WeatherDescription.VeryHot => "images/avatars/very-hot-weather.jpg",

                // Group 14: Hot weather (20-30°C)
                // TShirt, Shorts, SunHat, Sandals
                WeatherDescription.Hot => "images/avatars/hot-weather.jpg",

                // Group 15: Normal/Default weather
                // NormalClothing
                WeatherDescription.Normal => "images/avatars/normal-weather.jpg",

                // Default fallback
                _ => "images/avatars/default.jpg"
            };
        }
    }
}
