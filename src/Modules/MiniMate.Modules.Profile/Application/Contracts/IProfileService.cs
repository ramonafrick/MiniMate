using MiniMate.Modules.Profile.Application.Models;

namespace MiniMate.Modules.Profile.Application.Contracts
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
