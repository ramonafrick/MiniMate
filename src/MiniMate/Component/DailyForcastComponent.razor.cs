using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MiniMate.Weather.Contracts;
using MiniMate.Weather.Models;
using MiniMate.Weather.Resources;

namespace MiniMate.Component
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
            var maxRain = HourlyForecast.Max(h => h.Rain);
            var maxPrecip = HourlyForecast.Max(h => h.Precipitation);
            return Math.Max(maxRain, maxPrecip);
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
            if (max == 0 || value == 0) return "0%";
            return $"{(value / max * 100):F0}%";
        }
        #endregion
    }
}
