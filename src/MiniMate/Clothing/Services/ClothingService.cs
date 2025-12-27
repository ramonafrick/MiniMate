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
            var apparentTemp = weatherData.Current.ApparentTemperature;

            // Check if it's foggy (WMO codes 45-48)
            var isFoggy = weatherCode >= 45 && weatherCode <= 48;

            // Calculate wind chill effect: significant if feels like 3°C colder or more
            var hasSignificantWindChill = (temp - apparentTemp) >= 3;

            // Determine clothing based on temperature and weather conditions
            return (temp, rain, snow, windSpeed, weatherCode) switch
            {
                // Very cold (below -10°C) with snow
                ( < -10, _, > 0, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.ExtremeColdWithSnow,
                    Description = WeatherDescription.ExtremeColdWithSnow.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.WinterJacket, ClothingItem.Hat, ClothingItem.Scarf, ClothingItem.Gloves, ClothingItem.WinterBoots, ClothingItem.ThermalUnderwear }.Translate(_localizer),
                    Advice = WeatherAdvice.ExtremeColdStayInside.Translate(_localizer)
                },

                // Very cold (below -10°C)
                ( < -10, _, _, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.ExtremeCold,
                    Description = WeatherDescription.ExtremeCold.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.WinterJacket, ClothingItem.Hat, ClothingItem.Scarf, ClothingItem.Gloves, ClothingItem.WarmShoes }.Translate(_localizer),
                    Advice = WeatherAdvice.ExtremeColdLayerUp.Translate(_localizer)
                },

                // Cold with heavy snow (below 0°C)
                ( < 0, _, > 2, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.ColdWithHeavySnow,
                    Description = WeatherDescription.ColdWithHeavySnow.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.WinterJacket, ClothingItem.Hat, ClothingItem.Gloves, ClothingItem.WinterBoots, ClothingItem.Scarf }.Translate(_localizer),
                    Advice = WeatherAdvice.SnowWaterproofImportant.Translate(_localizer)
                },

                // Cold with snow (below 0°C)
                ( < 0, _, > 0, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.ColdWithSnow,
                    Description = WeatherDescription.ColdWithSnow.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.WinterJacket, ClothingItem.Hat, ClothingItem.Gloves, ClothingItem.WarmShoes }.Translate(_localizer),
                    Advice = WeatherAdvice.SnowDontForgetGloves.Translate(_localizer)
                },

                // Cold (below 0°C)
                ( < 0, _, _, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.Cold,
                    Description = WeatherDescription.Cold.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.WinterJacket, ClothingItem.Hat, ClothingItem.Scarf, ClothingItem.WarmShoes }.Translate(_localizer),
                    Advice = WeatherAdvice.ColdWarmJacketNeeded.Translate(_localizer)
                },

                // Cool with heavy rain (0-10°C)
                (>= 0 and < 10, > 5, _, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.CoolWithHeavyRain,
                    Description = WeatherDescription.CoolWithHeavyRain.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.RainJacket, ClothingItem.Umbrella, ClothingItem.WaterproofShoes, ClothingItem.Sweater }.Translate(_localizer),
                    Advice = WeatherAdvice.HeavyRainWaterproofImportant.Translate(_localizer)
                },

                // Cool with rain (0-10°C)
                (>= 0 and < 10, > 0, _, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.CoolWithRain,
                    Description = WeatherDescription.CoolWithRain.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.Jacket, ClothingItem.Umbrella, ClothingItem.Sweater, ClothingItem.WaterproofShoes }.Translate(_localizer),
                    Advice = WeatherAdvice.RainTakeUmbrella.Translate(_localizer)
                },

                // Cool and windy (0-10°C)
                (>= 0 and < 10, _, _, > 30, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.CoolAndWindy,
                    Description = WeatherDescription.CoolAndWindy.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.Windbreaker, ClothingItem.Sweater, ClothingItem.Scarf, ClothingItem.Hat, ClothingItem.Sneakers }.Translate(_localizer),
                    Advice = WeatherAdvice.WindyWindbreakerProtects.Translate(_localizer)
                },

                // Cool (0-10°C) - Check for fog or significant wind chill
                (>= 0 and < 10, _, _, _, _) when isFoggy || hasSignificantWindChill => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.Cool,
                    Description = WeatherDescription.Cool.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.Jacket, ClothingItem.Sweater, ClothingItem.LongPants, ClothingItem.Hat, ClothingItem.Scarf, ClothingItem.Sneakers }.Translate(_localizer),
                    Advice = WeatherAdvice.CoolButPleasantJacketEnough.Translate(_localizer)
                },

                // Cool (0-10°C) - Normal conditions
                (>= 0 and < 10, _, _, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.Cool,
                    Description = WeatherDescription.Cool.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.Jacket, ClothingItem.Sweater, ClothingItem.LongPants, ClothingItem.Sneakers }.Translate(_localizer),
                    Advice = WeatherAdvice.CoolButPleasantJacketEnough.Translate(_localizer)
                },

                // Mild with rain (10-15°C)
                (>= 10 and < 15, > 0, _, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.MildWithRain,
                    Description = WeatherDescription.MildWithRain.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.LightJacket, ClothingItem.Umbrella, ClothingItem.WaterproofShoes }.Translate(_localizer),
                    Advice = WeatherAdvice.RainButMildLightJacketEnough.Translate(_localizer)
                },

                // Mild (10-15°C)
                (>= 10 and < 15, _, _, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.Mild,
                    Description = WeatherDescription.Mild.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.LightJacket, ClothingItem.LongSleevShirt, ClothingItem.Sneakers }.Translate(_localizer),
                    Advice = WeatherAdvice.PleasantTemperatureLightJacketPerfect.Translate(_localizer)
                },

                // Warm with rain (15-20°C)
                (>= 15 and < 20, > 0, _, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.WarmWithRain,
                    Description = WeatherDescription.WarmWithRain.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.RainJacket, ClothingItem.Umbrella, ClothingItem.TShirt, ClothingItem.WaterproofShoes }.Translate(_localizer),
                    Advice = WeatherAdvice.WarmButWetUmbrellaEnough.Translate(_localizer)
                },

                // Warm (15-20°C)
                (>= 15 and < 20, _, _, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.Warm,
                    Description = WeatherDescription.Warm.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.LongSleevShirt, ClothingItem.TShirt, ClothingItem.Sneakers }.Translate(_localizer),
                    Advice = WeatherAdvice.NiceWeatherLightClothingIdeal.Translate(_localizer)
                },

                // Hot with thunderstorm (above 25°C)
                (>= 25, _, _, _, >= 95 and <= 99) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.HotWithThunderstorm,
                    Description = WeatherDescription.HotWithThunderstorm.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.TShirt, ClothingItem.Shorts, ClothingItem.WaterproofShoes }.Translate(_localizer),
                    Advice = WeatherAdvice.ThunderstormStayInside.Translate(_localizer)
                },

                // Hot with rain (above 25°C)
                (>= 25, > 0, _, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.HotWithRain,
                    Description = WeatherDescription.HotWithRain.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.TShirt, ClothingItem.Shorts, ClothingItem.Umbrella, ClothingItem.Sandals }.Translate(_localizer),
                    Advice = WeatherAdvice.WarmRainLightQuickDry.Translate(_localizer)
                },

                // Very hot (above 30°C)
                (>= 30, _, _, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.VeryHot,
                    Description = WeatherDescription.VeryHot.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.TShirt, ClothingItem.Shorts, ClothingItem.SunHat, ClothingItem.Sunglasses, ClothingItem.Sandals }.Translate(_localizer),
                    Advice = WeatherAdvice.VeryHotStayInShadeAndDrink.Translate(_localizer)
                },

                // Hot (20-30°C)
                (>= 20 and < 30, _, _, _, _) => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.Hot,
                    Description = WeatherDescription.Hot.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.TShirt, ClothingItem.Shorts, ClothingItem.SunHat, ClothingItem.Sandals }.Translate(_localizer),
                    Advice = WeatherAdvice.WarmAndSunnySunProtection.Translate(_localizer)
                },

                // Default fallback
                _ => new ClothingRecommendation
                {
                    WeatherDescription = WeatherDescription.Normal,
                    Description = WeatherDescription.Normal.Translate(_localizer),
                    ClothingItems = new[] { ClothingItem.NormalClothing }.Translate(_localizer),
                    Advice = WeatherAdvice.WeatherIsPleasant.Translate(_localizer)
                }
            };
        }
    }
}
