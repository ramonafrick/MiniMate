using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MiniMate.Modules.Location.Application.Contracts;
using MiniMate.Modules.Location.Domain;
using MiniMate.Modules.Location.Resources;

namespace MiniMate.Modules.Location.UI.Components
{
    public partial class LocationSearchComponent : ComponentBase, IDisposable
    {
        #region Properties
        [Inject] protected ILocationService LocationService { get; set; } = null!;
        [Inject] protected IStringLocalizer<LocationResources> Localizer { get; set; } = null!;
        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

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

        /// <summary>
        /// Two-way bindable selected location
        /// </summary>
        [Parameter]
        public LocationData? SelectedLocation { get; set; }

        /// <summary>
        /// Event callback for SelectedLocation changes (two-way binding)
        /// </summary>
        [Parameter]
        public EventCallback<LocationData?> SelectedLocationChanged { get; set; }

        protected string SearchQuery { get; set; } = "";
        protected LocationData[] SearchResults { get; set; } = [];
        protected bool IsLoadingLocation { get; set; } = false;
        protected bool ShowDropdown { get; set; } = false;
        protected string? ErrorMessage { get; set; }
        protected bool ShowLocationButton { get; set; } = true;

        private System.Timers.Timer? _searchTimer;
        #endregion

        #region Methods
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

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

            // Update SearchQuery if SelectedLocation is provided from parent
            if (SelectedLocation != null && string.IsNullOrEmpty(SearchQuery))
            {
                SearchQuery = SelectedLocation.DisplayName;
            }
            // Set the initial location name in the search box if provided
            else if (!string.IsNullOrEmpty(InitialLocationName) && string.IsNullOrEmpty(SearchQuery))
            {
                SearchQuery = InitialLocationName;
            }
        }

        protected void OnSearchInputChanged()
        {
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
                    SearchResults = await LocationService.SearchLocationAsync(SearchQuery);
                    ShowDropdown = SearchResults.Length > 0;
                    StateHasChanged();
                }
                catch
                {
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

            // Notify parent component via two-way binding
            if (SelectedLocationChanged.HasDelegate)
            {
                await SelectedLocationChanged.InvokeAsync(location);
            }

            // Notify parent component via callback
            if (OnLocationSelected.HasDelegate)
            {
                await OnLocationSelected.InvokeAsync(location);
            }

            StateHasChanged();
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
                    position = await JSRuntime.InvokeAsync<GeolocationPosition>("getCurrentPosition");
                }
                catch (JSException jsEx) when (jsEx.Message.ToLower().Contains("timeout"))
                {
                    // GPS timed out - try IP-based geolocation as fallback
                    position = await JSRuntime.InvokeAsync<GeolocationPosition>("getLocationFromIP");
                    isApproximate = true;
                }

                // Get actual location name from coordinates using reverse geocoding
                var locationName = await LocationService.GetLocationNameFromCoordinatesAsync(
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

                // Notify parent component via two-way binding
                if (SelectedLocationChanged.HasDelegate)
                {
                    await SelectedLocationChanged.InvokeAsync(SelectedLocation);
                }

                // Notify parent component via callback
                if (OnLocationSelected.HasDelegate)
                {
                    await OnLocationSelected.InvokeAsync(SelectedLocation);
                }

                StateHasChanged();

                // Show info message if location is approximate
                if (isApproximate)
                {
                    ErrorMessage = Localizer["LocationApproximate"];
                }
            }
            catch (JSException jsEx)
            {
                // JavaScript error - likely from geolocation API
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
            catch (Exception)
            {
                ErrorMessage = Localizer["GeolocationError"];
            }
            finally
            {
                IsLoadingLocation = false;
                StateHasChanged();
            }
        }

        public void Dispose()
        {
            _searchTimer?.Dispose();
        }
        #endregion
    }
}
