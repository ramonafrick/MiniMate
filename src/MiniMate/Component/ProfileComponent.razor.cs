using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MiniMate.Profile.Contracts;
using MiniMate.Profile.Models;
using MiniMate.Modules.Weather.Infrastructure.Models;
using System.Globalization;

namespace MiniMate.Component
{
    public partial class ProfileComponent : ComponentBase, IDisposable
    {
        #region Properties
        [Inject] protected IProfileService ProfileService { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

        protected string UserName { get; set; } = "";
        protected string SelectedLanguage { get; set; } = "de";
        protected bool ShowSuccessMessage { get; set; } = false;
        protected string? InitialLocationName { get; set; }

        private LocationData? _selectedLocation;
        private System.Timers.Timer? _messageTimer;
        #endregion

        #region Methods
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadProfile();
        }

        private async Task LoadProfile()
        {
            try
            {
                var profile = await ProfileService.GetProfileAsync();
                UserName = profile.Name;
                SelectedLanguage = profile.Language;
                InitialLocationName = profile.LocationName;

                // Set the selected location from profile if available
                if (profile.Latitude.HasValue && profile.Longitude.HasValue)
                {
                    _selectedLocation = new LocationData(
                        Id: 0,
                        Name: profile.LocationName ?? "",
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading profile: {ex.Message}");
            }
        }

        protected void HandleLocationSelected(LocationData location)
        {
            _selectedLocation = location;
        }

        protected async Task SaveProfile()
        {
            try
            {
                var profile = new UserProfile
                {
                    Name = UserName,
                    Language = SelectedLanguage,
                    Latitude = _selectedLocation?.Latitude,
                    Longitude = _selectedLocation?.Longitude,
                    LocationName = _selectedLocation?.DisplayName
                };

                await ProfileService.SaveProfileAsync(profile);

                // Save language to localStorage for culture setting
                await JSRuntime.InvokeVoidAsync("blazorCulture.set", SelectedLanguage);

                // Show success message
                ShowSuccessMessage = true;
                _messageTimer?.Dispose();
                _messageTimer = new System.Timers.Timer(3000);
                _messageTimer.Elapsed += (_, _) => InvokeAsync(() =>
                {
                    ShowSuccessMessage = false;
                    StateHasChanged();
                });
                _messageTimer.AutoReset = false;
                _messageTimer.Start();

                // Reload page if language changed to apply new culture
                var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                if (currentCulture != SelectedLanguage)
                {
                    await Task.Delay(1000); // Brief delay to show success message
                    Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving profile: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _messageTimer?.Dispose();
        }
        #endregion
    }
}
