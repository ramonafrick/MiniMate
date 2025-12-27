using Microsoft.JSInterop;
using MiniMate.Profile.Contracts;
using MiniMate.Profile.Models;
using System.Text.Json;

namespace MiniMate.Profile.Services
{
    /// <summary>
    /// Service for managing user profile data using localStorage
    /// </summary>
    public class ProfileService : IProfileService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string PROFILE_KEY = "minimate_user_profile";

        public ProfileService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Gets the current user profile from localStorage
        /// </summary>
        public async Task<UserProfile> GetProfileAsync()
        {
            try
            {
                var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", PROFILE_KEY);
                if (!string.IsNullOrEmpty(json))
                {
                    var profile = JsonSerializer.Deserialize<UserProfile>(json);
                    if (profile != null)
                    {
                        return profile;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading profile: {ex.Message}");
            }

            // Return default profile if none exists
            return new UserProfile
            {
                Name = "Max",
                Language = "de"
            };
        }

        /// <summary>
        /// Saves the user profile to localStorage
        /// </summary>
        public async Task SaveProfileAsync(UserProfile profile)
        {
            try
            {
                var json = JsonSerializer.Serialize(profile);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", PROFILE_KEY, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving profile: {ex.Message}");
            }
        }
    }
}
