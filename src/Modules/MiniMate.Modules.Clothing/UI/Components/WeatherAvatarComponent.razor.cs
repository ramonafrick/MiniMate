using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MiniMate.Modules.Clothing.Application.Contracts;
using MiniMate.Modules.Clothing.Domain;
using MiniMate.Modules.Clothing.Resources;
using MiniMate.Modules.Clothing.Infrastructure.Helpers;
using MiniMate.Modules.Weather.Domain;

namespace MiniMate.Modules.Clothing.UI.Components
{
    public partial class WeatherAvatarComponent : ComponentBase
    {
        #region Properties
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
            WeatherDescription = WeatherDescription.Normal,
            Description = "Standard",
            ClothingItems = Array.Empty<string>()
        };
        #endregion

        #region Methods
        /// <summary>
        /// Gets the avatar image path based on weather description
        /// </summary>
        protected string AvatarImagePath => AvatarMapper.GetAvatarPath(Recommendation.WeatherDescription);

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
        /// Gets the appropriate icon for a clothing item (emoji or SVG)
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
            if (IsMatch(item, "LongSleevShirt") || item.Contains("sleeve")) return "svg-longsleeve"; // Custom SVG long sleeve
            if (IsMatch(item, "Sweater")) return "svg-hoodie"; // Custom SVG hoodie

            if (IsMatch(item, "LongPants") || item.Contains("lange")) return "👖";
            if (IsMatch(item, "Shorts") || item.Contains("kurze")) return "🩳";

            if (IsMatch(item, "SunHat") || item.Contains("sun")) return "👒";
            if (IsMatch(item, "Hat") || item.Contains("mütze")) return "svg-beanie"; // Custom SVG

            if (IsMatch(item, "Scarf")) return "🧣";
            if (IsMatch(item, "Gloves")) return "🧤";
            if (IsMatch(item, "Sunglasses")) return "🕶️";
            if (IsMatch(item, "Umbrella")) return "☂️";

            if (IsMatch(item, "WinterBoots")) return "🥾";
            if (IsMatch(item, "WaterproofShoes")) return "🥾";
            if (IsMatch(item, "WarmShoes")) return "🥾";
            if (IsMatch(item, "Sneakers") || item.Contains("sneaker") || item.Contains("turnschuh")) return "👟";
            if (IsMatch(item, "Sandals") || item.Contains("sandal")) return "🩴";

            if (IsMatch(item, "ThermalUnderwear") || item.Contains("thermal")) return "🩲";

            return "👕"; // Default icon
        }

        /// <summary>
        /// Checks if this item uses an SVG icon instead of emoji
        /// </summary>
        protected bool IsCustomSvgIcon(string clothingItem)
        {
            return GetClothingIcon(clothingItem).StartsWith("svg-");
        }

        /// <summary>
        /// Gets the SVG icon name (without svg- prefix)
        /// </summary>
        protected string GetSvgIconName(string clothingItem)
        {
            var icon = GetClothingIcon(clothingItem);
            return icon.StartsWith("svg-") ? icon.Substring(4) : "";
        }


        /// <summary>
        /// Helper method to check if the item matches a localized resource key
        /// </summary>
        private bool IsMatch(string item, string resourceKey)
        {
            var localizedValue = Localizer[resourceKey].Value?.ToLower() ?? "";
            return !string.IsNullOrEmpty(localizedValue) && item.Contains(localizedValue);
        }
        #endregion
    }
}
