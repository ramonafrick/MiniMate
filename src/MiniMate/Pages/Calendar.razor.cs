using MiniMate.Modules.Weather.Infrastructure.Models; using MiniMate.Modules.Weather.Domain.Entities;

namespace MiniMate.Pages
{
    public partial class Calendar
    {
        #region Properties
        private WeatherData? WeatherData;
        private bool IsLoading = false;
        private string? ErrorMessageKey;
        #endregion

        #region Methods
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadProfileLocation();
        }

        private async Task LoadProfileLocation()
        {
            IsLoading = true;
            ErrorMessageKey = null;

            try
            {
                var profile = await profileService.GetProfileAsync();

                if (profile.Latitude.HasValue && profile.Longitude.HasValue)
                {
                    await LoadWeatherData(profile.Latitude.Value, profile.Longitude.Value);
                }
                else
                {
                    ErrorMessageKey = "NoLocationSet";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading profile: {ex.Message}");
                ErrorMessageKey = "ErrorLoadingData";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadWeatherData(double latitude, double longitude)
        {
            try
            {
                WeatherData = await weatherService.GetCurrentWeatherAsync(latitude, longitude);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading weather data: {ex.Message}");
                ErrorMessageKey = "ErrorLoadingData";
            }
        }
        #endregion
    }
}
