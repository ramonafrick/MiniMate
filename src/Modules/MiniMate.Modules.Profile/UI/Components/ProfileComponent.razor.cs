using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MiniMate.Modules.Location.Domain;
using MiniMate.Modules.Profile.Application.Contracts;
using MiniMate.Modules.Profile.Application.Models;
using MiniMate.Modules.Profile.Resources;
using System.Globalization;

namespace MiniMate.Modules.Profile.UI.Components
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

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            // React to parameter changes if InitialLocationName is set from outside
            // This ensures proper data flow when parameters change
            Console.WriteLine($"ProfileComponent: OnParametersSet called - InitialLocationName='{InitialLocationName}'");
        }

        private async Task LoadProfile()
        {
            try
            {
                Console.WriteLine("ProfileComponent: Starting to load profile...");
                var profile = await ProfileService.GetProfileAsync();
                Console.WriteLine($"ProfileComponent: Loaded profile - Name: '{profile.Name}', Language: '{profile.Language}'");

                await InvokeAsync(() =>
                {
                    UserName = profile.Name;
                    SelectedLanguage = profile.Language;
                    InitialLocationName = profile.LocationName;

                    Console.WriteLine($"ProfileComponent: Set UserName='{UserName}', SelectedLanguage='{SelectedLanguage}'");

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

                    Console.WriteLine("ProfileComponent: Profile loaded successfully, triggering StateHasChanged");
                    StateHasChanged();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading profile: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        protected async Task HandleLocationSelected(LocationData location)
        {
            await InvokeAsync(() =>
            {
                _selectedLocation = location;
                Console.WriteLine($"ProfileComponent: Location selected - {location.DisplayName}");
                StateHasChanged();
            });
        }

        protected void HandleLanguageChange(ChangeEventArgs e)
        {
            SelectedLanguage = e.Value?.ToString() ?? "de";
            Console.WriteLine($"ProfileComponent: Language changed to '{SelectedLanguage}'");
            StateHasChanged();
        }

        protected void TestClick()
        {
            Console.WriteLine("ProfileComponent: TEST BUTTON CLICKED!");
        }

        protected async Task SaveProfile()
        {
            try
            {
                Console.WriteLine("ProfileComponent: SaveProfile button clicked");
                Console.WriteLine($"ProfileComponent: Current values - UserName='{UserName}', SelectedLanguage='{SelectedLanguage}'");

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

                Console.WriteLine("ProfileComponent: Profile saved successfully");

                // Reload page if language changed to apply new culture
                var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                if (currentCulture != SelectedLanguage)
                {
                    Console.WriteLine($"ProfileComponent: Language changed from '{currentCulture}' to '{SelectedLanguage}', reloading page");
                    await Task.Delay(1000); // Brief delay to show success message
                    Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving profile: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        public void Dispose()
        {
            _messageTimer?.Dispose();
        }
        #endregion
    }
}
