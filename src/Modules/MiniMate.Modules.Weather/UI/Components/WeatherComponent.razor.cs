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

        /// <summary>
        /// Flag to track if user manually selected a location (overrides InitialLocation)
        /// </summary>
        private bool _userSelectedLocation = false;

        /// <summary>
        /// Last InitialLocation that was loaded (to detect profile changes)
        /// </summary>
        private LocationData? _lastInitialLocation = null;
        #endregion

        #region Methods
        protected override async Task OnParametersSetAsync()
        {
            // Check if InitialLocation has changed from profile (e.g., user changed profile location)
            bool profileLocationChanged = InitialLocation != null && _lastInitialLocation != null &&
                (InitialLocation.Latitude != _lastInitialLocation.Latitude ||
                 InitialLocation.Longitude != _lastInitialLocation.Longitude);

            // Reset user override if profile location changed
            if (profileLocationChanged)
            {
                _userSelectedLocation = false;
            }

            // Only load InitialLocation if user hasn't manually selected a location
            if (InitialLocation != null && !_userSelectedLocation)
            {
                // Check if location has changed
                bool locationChanged = SelectedLocation == null ||
                    SelectedLocation.Latitude != InitialLocation.Latitude ||
                    SelectedLocation.Longitude != InitialLocation.Longitude;

                if (locationChanged)
                {
                    SelectedLocation = InitialLocation;
                    _lastInitialLocation = InitialLocation;
                    await LoadWeatherData(InitialLocation.Latitude, InitialLocation.Longitude);
                }
            }
        }

        protected async Task HandleLocationSelected(LocationData location)
        {
            // Mark that user has manually selected a location
            _userSelectedLocation = true;
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
