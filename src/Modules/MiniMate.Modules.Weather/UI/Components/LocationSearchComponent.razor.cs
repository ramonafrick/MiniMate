using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MiniMate.Modules.Weather.Application.Contracts;
using MiniMate.Modules.Weather.Domain.ValueObjects;
using MiniMate.Modules.Weather.Infrastructure.Models;
using MiniMate.Modules.Weather.Resources;

namespace MiniMate.Modules.Weather.UI.Components
{
    public partial class LocationSearchComponent : ComponentBase, IDisposable
    {
        #region Properties
        [Inject] protected IWeatherService WeatherService { get; set; } = null!;
        [Inject] protected IStringLocalizer<WeatherResources> Localizer { get; set; } = null!;

        /// <summary>
        /// Callback that is invoked when a location is selected
        /// </summary>
        [Parameter]
        public EventCallback<LocationData> OnLocationSelected { get; set; }

        /// <summary>
        /// Initial location name to display in the search box
        /// </summary>
        [Parameter]
        public string? InitialLocationName { get; set; }

        protected string SearchQuery { get; set; } = "";
        protected LocationData[] SearchResults { get; set; } = [];
        protected LocationData? SelectedLocation { get; set; }
        protected bool IsLoadingLocation { get; set; } = false;
        protected bool ShowDropdown { get; set; } = false;
        protected string? ErrorMessage { get; set; }
        protected bool ShowLocationButton { get; set; } = true;

        private System.Timers.Timer? _searchTimer;
        #endregion

        #region Methods
        protected override async Task OnInitializedAsync()
        {
            base.OnInitializedAsync();

            // Check if geolocation is available on this device
            try
            {
                ShowLocationButton = await JSRuntime.InvokeAsync<bool>("isGeolocationAvailable");
            }
            catch
            {
                ShowLocationButton = false;
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            // Set the initial location name in the search box if provided
            if (!string.IsNullOrEmpty(InitialLocationName) && string.IsNullOrEmpty(SearchQuery))
            {
                SearchQuery = InitialLocationName;
            }
        }

        protected void OnSearchInput(ChangeEventArgs e)
        {
            SearchQuery = e.Value?.ToString() ?? "";

            // Reset timer for debouncing
            _searchTimer?.Stop();
            _searchTimer?.Dispose();

            if (string.IsNullOrWhiteSpace(SearchQuery) || SearchQuery.Length < 2)
            {
                SearchResults = [];
                ShowDropdown = false;
                return;
            }

            // Debounce search - wait 300ms before searching
            _searchTimer = new System.Timers.Timer(300);
            _searchTimer.Elapsed += async (_, _) => await SearchLocations();
            _searchTimer.AutoReset = false;
            _searchTimer.Start();
        }

        private async Task SearchLocations()
        {
            await InvokeAsync(async () =>
            {
                try
                {
                    SearchResults = await WeatherService.SearchLocationAsync(SearchQuery);
                    ShowDropdown = SearchResults.Length > 0;
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error searching locations: {ex.Message}");
                    SearchResults = [];
                    ShowDropdown = false;
                    StateHasChanged();
                }
            });
        }

        protected void HandleKeyPress(KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case "Enter" when SearchResults.Length > 0:
                    _ = SelectLocation(SearchResults[0]);
                    break;
                case "Escape":
                    ShowDropdown = false;
                    break;
            }
        }

        protected async Task SelectLocation(LocationData location)
        {
            SelectedLocation = location;
            SearchQuery = location.DisplayName;
            ShowDropdown = false;
            SearchResults = [];

            // Notify parent component
            await OnLocationSelected.InvokeAsync(location);
        }

        protected async Task GetCurrentLocation()
        {
            IsLoadingLocation = true;
            ErrorMessage = null;

            try
            {
                GeolocationPosition position;
                bool isApproximate = false;

                try
                {
                    // First try GPS/browser geolocation
                    Console.WriteLine("Trying GPS/browser geolocation...");
                    position = await JSRuntime.InvokeAsync<GeolocationPosition>("getCurrentPosition");
                    Console.WriteLine($"GPS position received: Lat={position.Coords.Latitude}, Lon={position.Coords.Longitude}");
                }
                catch (JSException jsEx) when (jsEx.Message.ToLower().Contains("timeout"))
                {
                    // GPS timed out - try IP-based geolocation as fallback
                    Console.WriteLine("GPS timeout - trying IP-based geolocation...");
                    position = await JSRuntime.InvokeAsync<GeolocationPosition>("getLocationFromIP");
                    isApproximate = true;
                    Console.WriteLine($"IP-based position received: Lat={position.Coords.Latitude}, Lon={position.Coords.Longitude}");
                }

                // Get actual location name from coordinates using reverse geocoding
                var locationName = await WeatherService.GetLocationNameFromCoordinatesAsync(
                    position.Coords.Latitude,
                    position.Coords.Longitude
                );

                // Add indicator if location is approximate
                if (isApproximate)
                {
                    locationName = $"≈ {locationName}";
                }

                SelectedLocation = new LocationData(
                    Id: 0,
                    Name: locationName,
                    Latitude: position.Coords.Latitude,
                    Longitude: position.Coords.Longitude,
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

                SearchQuery = locationName;

                // Notify parent component
                await OnLocationSelected.InvokeAsync(SelectedLocation);

                // Show info message if location is approximate
                if (isApproximate)
                {
                    ErrorMessage = Localizer["LocationApproximate"];
                }
            }
            catch (JSException jsEx)
            {
                // JavaScript error - likely from geolocation API
                Console.WriteLine($"Geolocation JS error: {jsEx.Message}");

                // Determine the specific error type and use localized message
                var message = jsEx.Message.ToLower();
                if (message.Contains("denied"))
                {
                    ErrorMessage = Localizer["GeolocationPermissionDenied"];
                }
                else if (message.Contains("unavailable"))
                {
                    ErrorMessage = Localizer["GeolocationUnavailable"];
                }
                else
                {
                    ErrorMessage = Localizer["GeolocationError"];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Geolocation error: {ex.Message}");
                ErrorMessage = Localizer["GeolocationError"];
            }
            finally
            {
                IsLoadingLocation = false;
            }
        }

        public void Dispose()
        {
            _searchTimer?.Dispose();
        }
        #endregion
    }
}
