using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MiniMate.Clothing.Contracts;
using MiniMate.Clothing.Models;
using MiniMate.Clothing.Resources;
using MiniMate.Weather.Models;

namespace MiniMate.Component
{
    public partial class WeatherAvatarComponent : ComponentBase
    {
        [Inject] protected IClothingService ClothingService { get; set; } = null!;
        [Inject] protected IStringLocalizer<ClothingResources> Localizer { get; set; } = null!;

        /// <summary>
        /// Weather data from parent component
        /// </summary>
        [Parameter]
        public WeatherData? WeatherData { get; set; }

        /// <summary>
        /// Current clothing recommendation based on weather
        /// </summary>
        protected ClothingRecommendation Recommendation { get; set; } = new()
        {
            ImagePath = "images/avatars/default.jpg",
            Description = "Standard",
            ClothingItems = Array.Empty<string>()
        };

        /// <summary>
        /// Called when parameters are set
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (WeatherData != null)
            {
                Recommendation = ClothingService.GetClothingRecommendation(WeatherData);
            }
        }

        /// <summary>
        /// Handles image loading errors by showing a fallback image
        /// </summary>
        protected void HandleImageError()
        {
            // This will be called if the image fails to load
            Console.WriteLine($"Failed to load image: {Recommendation.ImagePath}");
        }

        /// <summary>
        /// Gets the appropriate emoji icon for a clothing item
        /// Uses normalized comparison to work with all translations
        /// </summary>
        protected string GetClothingIcon(string clothingItem)
        {
            var item = clothingItem.ToLower();

            // Check against localized strings (works for both DE and EN)
            if (IsMatch(item, "WinterJacket") || item.Contains("winter")) return "🧥";
            if (IsMatch(item, "RainJacket") || item.Contains("rain")) return "🧥";
            if (IsMatch(item, "Windbreaker") || item.Contains("wind")) return "🧥";
            if (IsMatch(item, "Jacket") || IsMatch(item, "LightJacket")) return "🧥";

            if (IsMatch(item, "TShirt") || item.Contains("t-shirt")) return "👕";
            if (IsMatch(item, "LongSleevShirt") || item.Contains("sleeve")) return "👕";
            if (IsMatch(item, "Sweater")) return "👔";

            if (IsMatch(item, "LongPants") || item.Contains("lange")) return "👖";
            if (IsMatch(item, "Shorts") || item.Contains("kurze")) return "🩳";

            if (IsMatch(item, "Hat") || item.Contains("mütze")) return "🧢";
            if (IsMatch(item, "SunHat") || item.Contains("sun")) return "👒";

            if (IsMatch(item, "Scarf")) return "🧣";
            if (IsMatch(item, "Gloves")) return "🧤";
            if (IsMatch(item, "Sunglasses")) return "🕶️";
            if (IsMatch(item, "Umbrella")) return "☂️";

            if (IsMatch(item, "WinterBoots")) return "🥾";
            if (IsMatch(item, "WaterproofShoes")) return "🥾";
            if (IsMatch(item, "WarmShoes")) return "👞";

            if (IsMatch(item, "ThermalUnderwear") || item.Contains("thermal")) return "🩲";

            return "👕"; // Default icon
        }

        /// <summary>
        /// Helper method to check if the item matches a localized resource key
        /// </summary>
        private bool IsMatch(string item, string resourceKey)
        {
            var localizedValue = Localizer[resourceKey].Value?.ToLower() ?? "";
            return !string.IsNullOrEmpty(localizedValue) && item.Contains(localizedValue);
        }
    }
}
