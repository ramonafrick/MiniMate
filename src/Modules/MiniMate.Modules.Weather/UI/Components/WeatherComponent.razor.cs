using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MiniMate.Modules.Weather.Application.Contracts;
using MiniMate.Modules.Weather.Domain.Entities;
using MiniMate.Modules.Weather.Infrastructure.Models;
using MiniMate.Modules.Weather.Resources;
using MiniMate.Shared.Kernel.Contracts;

namespace MiniMate.Modules.Weather.UI.Components
{
    public partial class WeatherComponent : ComponentBase
    {
        #region Properties
        [Inject] protected IWeatherService WeatherService { get; set; } = null!;
        [Inject] protected IStringLocalizer<WeatherResources> Localizer { get; set; } = null!;
        [Inject] protected IProfileService ProfileService { get; set; } = null!;

        protected LocationData? SelectedLocation { get; set; }
        protected WeatherData? WeatherData { get; set; }
        protected bool IsLoading { get; set; } = false;
        protected string? ErrorMessage { get; set; }
        #endregion

        #region Methods
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadDefaultLocation();
        }

        private async Task LoadDefaultLocation()
        {
            try
            {
                var profile = await ProfileService.GetProfileAsync();

                if (profile.Latitude.HasValue && profile.Longitude.HasValue)
                {
                    // Create a LocationData from profile
                    SelectedLocation = new LocationData(
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

                    await LoadWeatherData(profile.Latitude.Value, profile.Longitude.Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading default location: {ex.Message}");
            }
        }

        protected async Task HandleLocationSelected(LocationData location)
        {
            SelectedLocation = location;
            await LoadWeatherData(location.Latitude, location.Longitude);
        }

        private async Task LoadWeatherData(double latitude, double longitude)
        {
            IsLoading = true;
            ErrorMessage = null;
            WeatherData = null;

            try
            {
                WeatherData = await WeatherService.GetCurrentWeatherAsync(latitude, longitude);
                if (WeatherData == null)
                {
                    ErrorMessage = Localizer["ErrorLoadingWeatherData"];
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Localizer["ErrorLoadingWeatherDataWithMessage", ex.Message];
                Console.WriteLine($"Weather loading error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected async Task RetryWeatherLoad()
        {
            if (SelectedLocation != null)
            {
                await LoadWeatherData(SelectedLocation.Latitude, SelectedLocation.Longitude);
            }
        }

        #endregion
    }
}
