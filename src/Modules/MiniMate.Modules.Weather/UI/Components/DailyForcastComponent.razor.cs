using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MiniMate.Modules.Weather.Application.Contracts;
using MiniMate.Modules.Weather.Domain;
using MiniMate.Modules.Weather.Resources;

namespace MiniMate.Modules.Weather.UI.Components
{
    public partial class DailyForcastComponent : ComponentBase
    {
        #region Properties
        [Inject] protected IWeatherService WeatherService { get; set; } = null!;
        [Inject] protected IStringLocalizer<WeatherResources> Localizer { get; set; } = null!;

        [Parameter] public double Latitude { get; set; }
        [Parameter] public double Longitude { get; set; }

        protected HourlyForecastData[] HourlyForecast { get; set; } = [];
        protected bool IsLoading { get; set; } = false;
        protected string? ErrorMessage { get; set; }
        #endregion

        #region Lifecycle Methods
        protected override async Task OnParametersSetAsync()
        {
            if (Latitude != 0 || Longitude != 0)
            {
                await LoadForecastData();
            }
        }
        #endregion

        #region Methods
        private async Task LoadForecastData()
        {
            IsLoading = true;
            ErrorMessage = null;
            HourlyForecast = [];

            try
            {
                HourlyForecast = await WeatherService.GetHourlyForecastAsync(Latitude, Longitude);

                if (HourlyForecast.Length == 0)
                {
                    ErrorMessage = Localizer["Forecast_NoDataAvailable"];
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Localizer["Forecast_ErrorLoading", ex.Message];
                Console.WriteLine($"Forecast loading error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected double GetMaxRain()
        {
            if (HourlyForecast.Length == 0) return 0;
            // Calculate total rain including showers for each hour and get the maximum
            return HourlyForecast.Max(h => h.Rain + h.Showers);
        }

        protected double GetTotalRain(HourlyForecastData hour)
        {
            // Total rain = rain + showers
            return hour.Rain + hour.Showers;
        }

        protected string GetRainBarColor(double rainAmount)
        {
            // Return gradient color based on rain intensity
            return rainAmount switch
            {
                <= 0 => "",
                <= 1 => "linear-gradient(to top, #b3d9f2 0%, #cce5f6 50%, #e6f2fa 100%)", // Very light blue - drizzle
                <= 3 => "linear-gradient(to top, #74b9e6 0%, #8bc5ea 50%, #a3d1ee 100%)", // Light blue - light rain
                <= 5 => "linear-gradient(to top, #4a9fd5 0%, #5dade2 50%, #74b9e6 100%)", // Medium blue - moderate rain
                <= 7 => "linear-gradient(to top, #3498db 0%, #4a9fd5 50%, #5dade2 100%)", // Blue - heavy rain
                _ => "linear-gradient(to top, #2471a3 0%, #3498db 50%, #4a9fd5 100%)"     // Dark blue - very heavy rain
            };
        }

        protected string GetRainTextColor(double rainAmount)
        {
            // Return text color that contrasts well with the rain bar
            return rainAmount switch
            {
                <= 0 => "#1a5490",
                <= 3 => "#1a5490",  // Dark blue text for light backgrounds
                _ => "#ffffff"      // White text for darker backgrounds
            };
        }

        protected double GetMaxTemperature()
        {
            if (HourlyForecast.Length == 0) return 0;
            return HourlyForecast.Max(h => h.Temperature);
        }

        protected double GetMinTemperature()
        {
            if (HourlyForecast.Length == 0) return 0;
            return HourlyForecast.Min(h => h.Temperature);
        }

        protected string GetBarHeight(double value, double max)
        {
            if (value == 0) return "0%";

            // Use absolute scale with 10mm/h as maximum for better comparison
            // This helps children understand rain intensity better
            const double absoluteMax = 10.0;

            // Calculate percentage based on absolute scale
            var percentage = (value / absoluteMax * 100);

            // Cap at 100% for extreme rain values
            return $"{Math.Min(percentage, 100):F0}%";
        }
        #endregion
    }
}
