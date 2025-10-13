using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MiniMate.Weather.Contracts;
using MiniMate.Weather.Helper;
using MiniMate.Weather.Models;
using MiniMate.Weather.Resources;
using System.Globalization;

namespace MiniMate.Component
{
    public partial class WeatherComponent : ComponentBase, IDisposable
    {
        #region Properties
        [Inject] protected IWeatherService WeatherService { get; set; } = null!;
        [Inject] protected IStringLocalizer<WeatherResources> Localizer { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        protected string SearchQuery { get; set; } = "";
        protected LocationData[] SearchResults { get; set; } = [];
        protected LocationData? SelectedLocation { get; set; }
        protected WeatherData? WeatherData { get; set; }
        protected bool IsLoading { get; set; } = false;
        protected bool IsLoadingLocation { get; set; } = false;
        protected bool ShowDropdown { get; set; } = false;
        protected string? ErrorMessage { get; set; }
        protected string CurrentLanguage => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToUpper();
        private System.Timers.Timer? _searchTimer;
        #endregion

        #region Methods
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
            await LoadWeatherData(location.Latitude, location.Longitude);
        }

        protected async Task GetCurrentLocation()
        {
            IsLoadingLocation = true;
            ErrorMessage = null;

            try
            {
                var position = await JSRuntime.InvokeAsync<GeolocationPosition>("getCurrentPosition");
                SelectedLocation = new LocationData(
                    Id: 0,
                    Name: Localizer["MyLocationText"],
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

                SearchQuery = Localizer["MyLocationText"];
                await LoadWeatherData(position.Coords.Latitude, position.Coords.Longitude);
            }
            catch (Exception ex)
            {
                ErrorMessage = Localizer["ErrorMessageWeatherLocation"];
                Console.WriteLine($"Geolocation error: {ex.Message}");
            }
            finally
            {
                IsLoadingLocation = false;
            }
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
                    ErrorMessage = "Wetterdaten konnten nicht geladen werden. Bitte versuchen Sie es später erneut.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fehler beim Laden der Wetterdaten: {ex.Message}";
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

        protected async Task ToggleLanguage()
        {
            var newCulture = CurrentLanguage == "DE" ? "en" : "de";

            // Save culture to localStorage
            await JSRuntime.InvokeVoidAsync("blazorCulture.set", newCulture);

            // Reload page to apply new culture
            Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
        }

        public void Dispose()
        {
            _searchTimer?.Dispose();
        }
        #endregion
    }
}
