using MiniMate.Profile.Models;

namespace MiniMate.Profile.Contracts
{
    /// <summary>
    /// Service for managing user profile data
    /// </summary>
    public interface IProfileService
    {
        /// <summary>
        /// Gets the current user profile
        /// </summary>
        Task<UserProfile> GetProfileAsync();

        /// <summary>
        /// Saves the user profile to local storage
        /// </summary>
        Task SaveProfileAsync(UserProfile profile);
    }
}
