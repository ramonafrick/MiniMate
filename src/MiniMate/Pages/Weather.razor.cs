using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MiniMate.Modules.Weather.Resources;
using MiniMate.Modules.Weather.UI.Components;
using MiniMate.Modules.Weather.Domain;
using MiniMate.Modules.Location.Domain;
using MiniMate.Profile.Contracts;

namespace MiniMate.Pages
{
    public partial class Weather : ComponentBase
    {
        #region Properties
        [Inject] protected IStringLocalizer<WeatherResources> Localizer { get; set; } = null!;
        [Inject] protected IProfileService ProfileService { get; set; } = null!;

        protected LocationData? ProfileLocation { get; set; }

        private WeatherComponent? weatherComponent;
        private WeatherData? currentWeatherData;
        #endregion

        #region Lifecycle Methods
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadProfileLocation();
        }
        #endregion

        #region Methods
        private async Task LoadProfileLocation()
        {
            try
            {
                var profile = await ProfileService.GetProfileAsync();

                if (profile.Latitude.HasValue && profile.Longitude.HasValue)
                {
                    // Create LocationData from profile
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading profile location: {ex.Message}");
            }
        }

        protected void HandleWeatherDataLoaded(WeatherData? weatherData)
        {
            currentWeatherData = weatherData;
            StateHasChanged();
        }
        #endregion
    }
}
