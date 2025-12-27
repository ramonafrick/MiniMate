using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MiniMate.Modules.Weather.Application.Contracts;
using MiniMate.Modules.Weather.Domain;
using MiniMate.Modules.Weather.Resources;
using MiniMate.Modules.Location.Domain;

namespace MiniMate.Modules.Weather.UI.Components
{
    public partial class WeatherComponent : ComponentBase
    {
        #region Properties
        [Inject] protected IWeatherService WeatherService { get; set; } = null!;
        [Inject] protected IStringLocalizer<WeatherResources> Localizer { get; set; } = null!;

        /// <summary>
        /// Initial location from profile (set by parent page)
        /// </summary>
        [Parameter]
        public LocationData? InitialLocation { get; set; }

        protected LocationData? SelectedLocation { get; set; }

        /// <summary>
        /// Current weather data - made public so parent page can access it
        /// </summary>
        public WeatherData? WeatherData { get; set; }

        /// <summary>
        /// Callback invoked when weather data is loaded
        /// </summary>
        [Parameter]
        public EventCallback<WeatherData?> OnWeatherDataLoaded { get; set; }

        /// <summary>
        /// Whether to show the title in the component
        /// </summary>
        [Parameter]
        public bool ShowTitle { get; set; } = true;

        protected bool IsLoading { get; set; } = false;
        protected string? ErrorMessage { get; set; }
        #endregion

        #region Methods
        protected override async Task OnParametersSetAsync()
        {
            // Load weather for initial location if provided and no location selected yet
            if (InitialLocation != null && SelectedLocation == null)
            {
                SelectedLocation = InitialLocation;
                await LoadWeatherData(InitialLocation.Latitude, InitialLocation.Longitude);
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
                else
                {
                    await OnWeatherDataLoaded.InvokeAsync(WeatherData);
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
