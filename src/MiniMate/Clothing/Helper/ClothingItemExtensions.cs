using Microsoft.Extensions.Localization;
using MiniMate.Clothing.Models;
using MiniMate.Clothing.Resources;

namespace MiniMate.Clothing.Helper
{
    /// <summary>
    /// Extension methods for ClothingItem enum
    /// </summary>
    public static class ClothingItemExtensions
    {
        /// <summary>
        /// Translates a ClothingItem to the current locale using the resource localizer
        /// </summary>
        /// <param name="item">The clothing item to translate</param>
        /// <param name="localizer">The string localizer for ClothingResources</param>
        /// <returns>Localized string for the clothing item</returns>
        public static string Translate(this ClothingItem item, IStringLocalizer<ClothingResources> localizer)
        {
            return item switch
            {
                // Winter clothing
                ClothingItem.WinterJacket => localizer["WinterJacket"],
                ClothingItem.Hat => localizer["Hat"],
                ClothingItem.Scarf => localizer["Scarf"],
                ClothingItem.Gloves => localizer["Gloves"],
                ClothingItem.WinterBoots => localizer["WinterBoots"],
                ClothingItem.ThermalUnderwear => localizer["ThermalUnderwear"],

                // Rain gear
                ClothingItem.RainJacket => localizer["RainJacket"],
                ClothingItem.Umbrella => localizer["Umbrella"],
                ClothingItem.WaterproofShoes => localizer["WaterproofShoes"],

                // Mid-season clothing
                ClothingItem.Jacket => localizer["Jacket"],
                ClothingItem.LightJacket => localizer["LightJacket"],
                ClothingItem.Sweater => localizer["Sweater"],
                ClothingItem.LongSleevShirt => localizer["LongSleevShirt"],
                ClothingItem.LongPants => localizer["LongPants"],

                // Wind protection
                ClothingItem.Windbreaker => localizer["Windbreaker"],

                // Summer clothing
                ClothingItem.TShirt => localizer["TShirt"],
                ClothingItem.Shorts => localizer["Shorts"],
                ClothingItem.SunHat => localizer["SunHat"],
                ClothingItem.Sunglasses => localizer["Sunglasses"],

                // General
                ClothingItem.WarmShoes => localizer["WarmShoes"],
                ClothingItem.NormalClothing => localizer["NormalClothing"],

                _ => item.ToString()
            };
        }

        /// <summary>
        /// Translates an array of ClothingItems to localized strings
        /// </summary>
        /// <param name="items">The clothing items to translate</param>
        /// <param name="localizer">The string localizer for ClothingResources</param>
        /// <returns>Array of localized strings</returns>
        public static string[] Translate(this ClothingItem[] items, IStringLocalizer<ClothingResources> localizer)
        {
            return items.Select(item => item.Translate(localizer)).ToArray();
        }

        /// <summary>
        /// Translates a WeatherDescription to the current locale using the resource localizer
        /// </summary>
        /// <param name="description">The weather description to translate</param>
        /// <param name="localizer">The string localizer for ClothingResources</param>
        /// <returns>Localized string for the weather description</returns>
        public static string Translate(this WeatherDescription description, IStringLocalizer<ClothingResources> localizer)
        {
            return description switch
            {
                WeatherDescription.ExtremeColdWithSnow => localizer["ExtremeColdWithSnow"],
                WeatherDescription.ExtremeCold => localizer["ExtremeCold"],
                WeatherDescription.ColdWithHeavySnow => localizer["ColdWithHeavySnow"],
                WeatherDescription.ColdWithSnow => localizer["ColdWithSnow"],
                WeatherDescription.Cold => localizer["Cold"],
                WeatherDescription.CoolWithHeavyRain => localizer["CoolWithHeavyRain"],
                WeatherDescription.CoolWithRain => localizer["CoolWithRain"],
                WeatherDescription.CoolAndWindy => localizer["CoolAndWindy"],
                WeatherDescription.Cool => localizer["Cool"],
                WeatherDescription.MildWithRain => localizer["MildWithRain"],
                WeatherDescription.Mild => localizer["Mild"],
                WeatherDescription.WarmWithRain => localizer["WarmWithRain"],
                WeatherDescription.Warm => localizer["Warm"],
                WeatherDescription.HotWithThunderstorm => localizer["HotWithThunderstorm"],
                WeatherDescription.HotWithRain => localizer["HotWithRain"],
                WeatherDescription.VeryHot => localizer["VeryHot"],
                WeatherDescription.Hot => localizer["Hot"],
                WeatherDescription.Normal => localizer["Normal"],
                _ => description.ToString()
            };
        }

        /// <summary>
        /// Translates a WeatherAdvice to the current locale using the resource localizer
        /// </summary>
        /// <param name="advice">The weather advice to translate</param>
        /// <param name="localizer">The string localizer for ClothingResources</param>
        /// <returns>Localized string for the weather advice</returns>
        public static string Translate(this WeatherAdvice advice, IStringLocalizer<ClothingResources> localizer)
        {
            return advice switch
            {
                WeatherAdvice.ExtremeColdStayInside => localizer["ExtremeColdStayInside"],
                WeatherAdvice.ExtremeColdLayerUp => localizer["ExtremeColdLayerUp"],
                WeatherAdvice.SnowWaterproofImportant => localizer["SnowWaterproofImportant"],
                WeatherAdvice.SnowDontForgetGloves => localizer["SnowDontForgetGloves"],
                WeatherAdvice.ColdWarmJacketNeeded => localizer["ColdWarmJacketNeeded"],
                WeatherAdvice.HeavyRainWaterproofImportant => localizer["HeavyRainWaterproofImportant"],
                WeatherAdvice.RainTakeUmbrella => localizer["RainTakeUmbrella"],
                WeatherAdvice.WindyWindbreakerProtects => localizer["WindyWindbreakerProtects"],
                WeatherAdvice.CoolButPleasantJacketEnough => localizer["CoolButPleasantJacketEnough"],
                WeatherAdvice.RainButMildLightJacketEnough => localizer["RainButMildLightJacketEnough"],
                WeatherAdvice.PleasantTemperatureLightJacketPerfect => localizer["PleasantTemperatureLightJacketPerfect"],
                WeatherAdvice.WarmButWetUmbrellaEnough => localizer["WarmButWetUmbrellaEnough"],
                WeatherAdvice.NiceWeatherLightClothingIdeal => localizer["NiceWeatherLightClothingIdeal"],
                WeatherAdvice.ThunderstormStayInside => localizer["ThunderstormStayInside"],
                WeatherAdvice.WarmRainLightQuickDry => localizer["WarmRainLightQuickDry"],
                WeatherAdvice.VeryHotStayInShadeAndDrink => localizer["VeryHotStayInShadeAndDrink"],
                WeatherAdvice.WarmAndSunnySunProtection => localizer["WarmAndSunnySunProtection"],
                WeatherAdvice.WeatherIsPleasant => localizer["WeatherIsPleasant"],
                _ => advice.ToString()
            };
        }
    }
}
