using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MiniMate.Clothing.Contracts;
using MiniMate.Clothing.Models;
using MiniMate.Weather.Models;
using MiniMate.Clothing.Resources;
using MiniMate.Clothing.Helper;

namespace MiniMate.Clothing.Services
{
    /// <summary>
    /// Service that determines appropriate clothing based on weather conditions
    /// </summary>
    public class ClothingService : IClothingService
    {
        private readonly IStringLocalizer<ClothingResources> _localizer;

        public ClothingService(IStringLocalizer<ClothingResources> localizer)
        {
            _localizer = localizer;
        }

        public ClothingRecommendation GetClothingRecommendation(WeatherData weatherData)
        {
            var temp = weatherData.Current.Temperature;
            var rain = weatherData.Current.Rain;
            var snow = weatherData.Current.Snowfall;
            var windSpeed = weatherData.Current.WindSpeed;
            var weatherCode = weatherData.Current.WeatherCode;

            // Determine clothing based on temperature and weather conditions
            return (temp, rain, snow, windSpeed, weatherCode) switch
            {
                // Very cold (below -10°C) with snow
                ( < -10, _, > 0, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/extreme-winter.jpg",
                    Description = WeatherDescription.ExtremeColdWithSnow.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.WinterJacket, ClothingItem.Hat, ClothingItem.Scarf, ClothingItem.Gloves, ClothingItem.WinterBoots, ClothingItem.ThermalUnderwear }.Translate(_localizer),
                    Advice = WeatherAdvice.ExtremeColdStayInside.Translate(_localizer)
                },

                // Very cold (below -10°C)
                ( < -10, _, _, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/extreme-cold.jpg",
                    Description = WeatherDescription.ExtremeCold.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.WinterJacket, ClothingItem.Hat, ClothingItem.Scarf, ClothingItem.Gloves, ClothingItem.WarmShoes }.Translate(_localizer),
                    Advice = WeatherAdvice.ExtremeColdLayerUp.Translate(_localizer)
                },

                // Cold with heavy snow (below 0°C)
                ( < 0, _, > 2, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/snow-day.jpg",
                    Description = WeatherDescription.ColdWithHeavySnow.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.WinterJacket, ClothingItem.Hat, ClothingItem.Gloves, ClothingItem.WinterBoots, ClothingItem.Scarf }.Translate(_localizer),
                    Advice = WeatherAdvice.SnowWaterproofImportant.Translate(_localizer)
                },

                // Cold with snow (below 0°C)
                ( < 0, _, > 0, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/winter-snow.jpg",
                    Description = WeatherDescription.ColdWithSnow.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.WinterJacket, ClothingItem.Hat, ClothingItem.Gloves, ClothingItem.WarmShoes }.Translate(_localizer),
                    Advice = WeatherAdvice.SnowDontForgetGloves.Translate(_localizer)
                },

                // Cold (below 0°C)
                ( < 0, _, _, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/winter.jpg",
                    Description = WeatherDescription.Cold.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.WinterJacket, ClothingItem.Hat, ClothingItem.Scarf, ClothingItem.WarmShoes }.Translate(_localizer),
                    Advice = WeatherAdvice.ColdWarmJacketNeeded.Translate(_localizer)
                },

                // Cool with heavy rain (0-10°C)
                (>= 0 and < 10, > 5, _, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/rainy-cold.jpg",
                    Description = WeatherDescription.CoolWithHeavyRain.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.RainJacket, ClothingItem.Umbrella, ClothingItem.WaterproofShoes, ClothingItem.Sweater }.Translate(_localizer),
                    Advice = WeatherAdvice.HeavyRainWaterproofImportant.Translate(_localizer)
                },

                // Cool with rain (0-10°C)
                (>= 0 and < 10, > 0, _, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/rainy-cool.jpg",
                    Description = WeatherDescription.CoolWithRain.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.Jacket, ClothingItem.Umbrella, ClothingItem.Sweater }.Translate(_localizer),
                    Advice = WeatherAdvice.RainTakeUmbrella.Translate(_localizer)
                },

                // Cool and windy (0-10°C)
                (>= 0 and < 10, _, _, > 30, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/windy-cool.jpg",
                    Description = WeatherDescription.CoolAndWindy.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.Windbreaker, ClothingItem.Sweater, ClothingItem.Scarf }.Translate(_localizer),
                    Advice = WeatherAdvice.WindyWindbreakerProtects.Translate(_localizer)
                },

                // Cool (0-10°C)
                (>= 0 and < 10, _, _, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/cool.jpg",
                    Description = WeatherDescription.Cool.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.Jacket, ClothingItem.Sweater, ClothingItem.LongPants }.Translate(_localizer),
                    Advice = WeatherAdvice.CoolButPleasantJacketEnough.Translate(_localizer)
                },

                // Mild with rain (10-15°C)
                (>= 10 and < 15, > 0, _, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/mild-rainy.jpg",
                    Description = WeatherDescription.MildWithRain.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.LightJacket, ClothingItem.Umbrella }.Translate(_localizer),
                    Advice = WeatherAdvice.RainButMildLightJacketEnough.Translate(_localizer)
                },

                // Mild (10-15°C)
                (>= 10 and < 15, _, _, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/mild.jpg",
                    Description = WeatherDescription.Mild.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.LightJacket, ClothingItem.LongSleevShirt }.Translate(_localizer),
                    Advice = WeatherAdvice.PleasantTemperatureLightJacketPerfect.Translate(_localizer)
                },

                // Warm with rain (15-20°C)
                (>= 15 and < 20, > 0, _, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/warm-rainy.jpg",
                    Description = WeatherDescription.WarmWithRain.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.RainJacket, ClothingItem.Umbrella, ClothingItem.TShirt }.Translate(_localizer),
                    Advice = WeatherAdvice.WarmButWetUmbrellaEnough.Translate(_localizer)
                },

                // Warm (15-20°C)
                (>= 15 and < 20, _, _, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/warm.jpg",
                    Description = WeatherDescription.Warm.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.LongSleevShirt, ClothingItem.TShirt }.Translate(_localizer),
                    Advice = WeatherAdvice.NiceWeatherLightClothingIdeal.Translate(_localizer)
                },

                // Hot with thunderstorm (above 25°C)
                (>= 25, _, _, _, >= 95 and <= 99) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/thunderstorm.jpg",
                    Description = WeatherDescription.HotWithThunderstorm.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.TShirt, ClothingItem.Shorts, ClothingItem.WaterproofShoes }.Translate(_localizer),
                    Advice = WeatherAdvice.ThunderstormStayInside.Translate(_localizer)
                },

                // Hot with rain (above 25°C)
                (>= 25, > 0, _, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/hot-rainy.jpg",
                    Description = WeatherDescription.HotWithRain.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.TShirt, ClothingItem.Shorts, ClothingItem.Umbrella }.Translate(_localizer),
                    Advice = WeatherAdvice.WarmRainLightQuickDry.Translate(_localizer)
                },

                // Very hot (above 30°C)
                (>= 30, _, _, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/very-hot.jpg",
                    Description = WeatherDescription.VeryHot.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.TShirt, ClothingItem.Shorts, ClothingItem.SunHat, ClothingItem.Sunglasses }.Translate(_localizer),
                    Advice = WeatherAdvice.VeryHotStayInShadeAndDrink.Translate(_localizer)
                },

                // Hot (20-30°C)
                (>= 20 and < 30, _, _, _, _) => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/hot.jpg",
                    Description = WeatherDescription.Hot.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.TShirt, ClothingItem.Shorts, ClothingItem.SunHat }.Translate(_localizer),
                    Advice = WeatherAdvice.WarmAndSunnySunProtection.Translate(_localizer)
                },

                // Default fallback
                _ => new ClothingRecommendation
                {
                    ImagePath = "images/avatars/default.jpg",
                    Description = WeatherDescription.Normal.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.NormalClothing }.Translate(_localizer),
                    Advice = WeatherAdvice.WeatherIsPleasant.Translate(_localizer)
                }
            };
        }
    }
}
