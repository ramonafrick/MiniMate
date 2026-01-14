using Microsoft.AspNetCore.Components;
using MiniMate.Modules.Weather.UI.Components;
using MiniMate.Modules.Weather.Domain;
using MiniMate.Modules.Profile.Application.Contracts;
using MiniMate.Modules.Profile.Application.Models;
using MiniMate.Modules.Profile.Application.Services;
using MiniMate.Modules.Location.Domain;
using System.Globalization;

namespace MiniMate.Maui.Components.Pages
{
    public partial class Weather : ComponentBase, IDisposable
    {
        [Inject] protected IProfileService ProfileService { get; set; } = null!;
        [Inject] protected ProfileStateService ProfileStateService { get; set; } = null!;
        [Inject] protected CultureStateService CultureStateService { get; set; } = null!;

        protected LocationData? ProfileLocation { get; set; }
        private WeatherData? currentWeatherData;
        private WeatherComponent? weatherComponent;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Console.WriteLine("Weather: OnInitializedAsync called");

            // Subscribe to ProfileStateService (Singleton - persists across navigation)
            ProfileStateService.ProfileStateChanged += OnProfileStateChanged;

            // Subscribe to CultureStateService for language changes
            CultureStateService.CultureChanged += OnCultureChanged;

            await LoadProfileLocation();
        }

        private void OnCultureChanged(object? sender, CultureInfo newCulture)
        {
            // Trigger re-render when culture changes
            Console.WriteLine($"Weather: Culture changed to {newCulture.Name}, triggering StateHasChanged");
            InvokeAsync(() => StateHasChanged());
        }

        private async Task LoadProfileLocation()
        {
            try
            {
                var profile = await ProfileService.GetProfileAsync();

                if (profile.Latitude.HasValue && profile.Longitude.HasValue)
                {
                    Console.WriteLine($"Weather: Loading location {profile.LocationName} ({profile.Latitude}, {profile.Longitude})");
                    ProfileLocation = new LocationData(
                        Id: 0,
                        Name: profile.LocationName ?? "Default Location",
                        Latitude: profile.Latitude.Value,
                        Longitude: profile.Longitude.Value,
                        Elevation: null,
                        FeatureCode: null,
                        CountryCode: null,
                        Admin1: null,
                        Admin2: null,
                        Admin3: null,
                        Admin4: null,
                        Timezone: null,
                        Population: null,
                        CountryId: null,
                        Country: null,
                        Postcodes: null
                    );
                    StateHasChanged();
                }
                else
                {
                    Console.WriteLine("Weather: No location in profile");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Weather: Error loading profile location: {ex.Message}");
            }
        }

        private void OnProfileStateChanged(object? sender, UserProfile profile)
        {
            // Reload location when profile state changes (via Singleton service)
            Console.WriteLine($"Weather: ProfileStateChanged event received - Location: {profile.LocationName}");
            InvokeAsync(async () =>
            {
                await LoadProfileLocation();
            });
        }

        protected void HandleWeatherDataLoaded(WeatherData? weatherData)
        {
            currentWeatherData = weatherData;
            StateHasChanged();
        }

        public void Dispose()
        {
            Console.WriteLine("Weather: Disposing - unsubscribing from ProfileStateChanged and CultureChanged");
            ProfileStateService.ProfileStateChanged -= OnProfileStateChanged;
            CultureStateService.CultureChanged -= OnCultureChanged;
        }
    }
}
