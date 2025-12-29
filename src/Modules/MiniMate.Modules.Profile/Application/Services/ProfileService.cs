using Microsoft.JSInterop;
using MiniMate.Modules.Profile.Application.Contracts;
using MiniMate.Modules.Profile.Application.Models;
using System.Text.Json;

namespace MiniMate.Modules.Profile.Application.Services
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
                Console.WriteLine($"ProfileService: Getting profile from localStorage with key '{PROFILE_KEY}'");
                var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", PROFILE_KEY);
                Console.WriteLine($"ProfileService: Retrieved JSON: {json ?? "null"}");

                if (!string.IsNullOrEmpty(json))
                {
                    var profile = JsonSerializer.Deserialize<UserProfile>(json);
                    if (profile != null)
                    {
                        Console.WriteLine($"ProfileService: Deserialized profile - Name: '{profile.Name}', Language: '{profile.Language}'");
                        return profile;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading profile: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            // Return default profile if none exists
            Console.WriteLine("ProfileService: Returning default profile");
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
                Console.WriteLine($"ProfileService: Saving profile - Name: '{profile.Name}', Language: '{profile.Language}'");
                var json = JsonSerializer.Serialize(profile);
                Console.WriteLine($"ProfileService: Serialized JSON: {json}");
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", PROFILE_KEY, json);
                Console.WriteLine($"ProfileService: Profile saved successfully to localStorage");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving profile: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}
