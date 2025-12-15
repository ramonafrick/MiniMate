using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MiniMate.Modules.Weather.Application.Contracts;
using MiniMate.Modules.Weather.Infrastructure.Models;
using MiniMate.Modules.Weather.Resources;

namespace MiniMate.Modules.Weather.UI.Components
{
    public partial class WeeklyForcastComponent : ComponentBase
    {
        #region Properties
        [Inject] protected IWeatherService WeatherService { get; set; } = null!;
        [Inject] protected IStringLocalizer<WeatherResources> Localizer { get; set; } = null!;

        [Parameter] public double Latitude { get; set; }
        [Parameter] public double Longitude { get; set; }

        protected DailyForecastData[] DailyForecast { get; set; } = [];
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
            DailyForecast = [];

            try
            {
                DailyForecast = await WeatherService.GetDailyForecastAsync(Latitude, Longitude);

                if (DailyForecast.Length == 0)
                {
                    ErrorMessage = Localizer["WeeklyForecast_NoDataAvailable"];
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Localizer["WeeklyForecast_ErrorLoading", ex.Message];
                Console.WriteLine($"Weekly forecast loading error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
        #endregion
    }
}
