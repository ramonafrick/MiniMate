using Microsoft.AspNetCore.Components;
using MiniMate.Modules.Weather.Application.Contracts;
using MiniMate.Modules.Profile.Application.Contracts;
using MiniMate.Modules.Profile.Application.Services;
using MiniMate.Modules.Weather.Domain;
using MiniMate.Modules.Profile.Application.Models;

namespace MiniMate.Maui.Components.Pages
{
    public partial class Calendar : ComponentBase, IDisposable
    {
        [Inject] protected IWeatherService weatherService { get; set; } = null!;
        [Inject] protected IProfileService profileService { get; set; } = null!;
        [Inject] protected ProfileStateService profileStateService { get; set; } = null!;

        private WeatherData? weatherData;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Console.WriteLine("Calendar: OnInitializedAsync called");

            // Subscribe to ProfileStateService (Singleton - persists across navigation)
            profileStateService.ProfileStateChanged += OnProfileStateChanged;

            // Load initial weather data
            await LoadWeatherDataFromProfile();
        }

        private async Task LoadWeatherDataFromProfile()
        {
            try
            {
                var profile = await profileService.GetProfileAsync();

                if (profile.Latitude.HasValue && profile.Longitude.HasValue)
                {
                    Console.WriteLine($"Calendar: Loading weather data for {profile.LocationName} ({profile.Latitude}, {profile.Longitude})");
                    weatherData = await weatherService.GetCurrentWeatherAsync(
                        profile.Latitude.Value,
                        profile.Longitude.Value);
                    StateHasChanged();
                }
                else
                {
                    Console.WriteLine("Calendar: No location in profile");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Calendar: Error loading weather data: {ex.Message}");
            }
        }

        private void OnProfileStateChanged(object? sender, UserProfile profile)
        {
            // Reload weather data when profile state changes (via Singleton service)
            Console.WriteLine($"Calendar: ProfileStateChanged event received - Location: {profile.LocationName}");
            InvokeAsync(async () =>
            {
                await LoadWeatherDataFromProfile();
            });
        }

        public void Dispose()
        {
            Console.WriteLine("Calendar: Disposing - unsubscribing from ProfileStateChanged");
            profileStateService.ProfileStateChanged -= OnProfileStateChanged;
        }
    }
}
