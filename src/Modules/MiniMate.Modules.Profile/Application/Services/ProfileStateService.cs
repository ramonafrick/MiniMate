using MiniMate.Modules.Profile.Application.Models;

namespace MiniMate.Modules.Profile.Application.Services
{
    /// <summary>
    /// Singleton service to hold and notify about profile state changes
    /// This service persists across navigation and component lifecycle
    /// </summary>
    public class ProfileStateService
    {
        private UserProfile? _currentProfile;

        /// <summary>
        /// Event raised when the profile state changes
        /// </summary>
        public event EventHandler<UserProfile>? ProfileStateChanged;

        /// <summary>
        /// Gets the current cached profile
        /// </summary>
        public UserProfile? CurrentProfile => _currentProfile;

        /// <summary>
        /// Updates the current profile state and notifies subscribers
        /// </summary>
        public void UpdateProfile(UserProfile profile)
        {
            _currentProfile = profile;
            Console.WriteLine($"ProfileStateService: Profile updated - Name: {profile.Name}");
            ProfileStateChanged?.Invoke(this, profile);
        }

        /// <summary>
        /// Clears the cached profile
        /// </summary>
        public void ClearProfile()
        {
            _currentProfile = null;
        }
    }
}
