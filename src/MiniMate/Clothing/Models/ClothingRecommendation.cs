namespace MiniMate.Clothing.Models
{
    /// <summary>
    /// Represents a clothing recommendation based on current weather conditions
    /// </summary>
    public record ClothingRecommendation
    {
        /// <summary>
        /// Weather description category for this recommendation.
        /// Used by AvatarMapper to display appropriate avatar image.
        /// </summary>
        public required WeatherDescription WeatherDescription { get; init; }

        /// <summary>
        /// Description of the clothing recommendation (localized)
        /// </summary>
        public required string Description { get; init; }

        /// <summary>
        /// List of recommended clothing items (localized)
        /// </summary>
        public required string[] ClothingItems { get; init; }

        /// <summary>
        /// Additional advice or warnings (localized)
        /// </summary>
        public string? Advice { get; init; }
    }
}
