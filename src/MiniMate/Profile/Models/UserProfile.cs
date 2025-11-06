namespace MiniMate.Profile.Models
{
    /// <summary>
    /// Represents user profile settings
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// The user's display name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The user's preferred language (e.g., "de", "en")
        /// </summary>
        public string Language { get; set; } = "de";

        /// <summary>
        /// The user's default location latitude
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// The user's default location longitude
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// The user's default location name
        /// </summary>
        public string? LocationName { get; set; }
    }
}
