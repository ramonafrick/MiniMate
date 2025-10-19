namespace MiniMate.Clothing.Models
{
    /// <summary>
    /// Represents a clothing recommendation based on current weather conditions
    /// </summary>
    public record ClothingRecommendation
    {
        /// <summary>
        /// Path to the avatar image file (e.g., "images/avatars/winter-jacket.jpg")
        /// </summary>
        public required string ImagePath { get; init; }

        /// <summary>
        /// Description of the clothing recommendation
        /// </summary>
        public required string Description { get; init; }

        /// <summary>
        /// List of recommended clothing items
        /// </summary>
        public required string[] ClothingItems { get; init; }

        /// <summary>
        /// Additional advice or warnings
        /// </summary>
        public string? Advice { get; init; }
    }
}
