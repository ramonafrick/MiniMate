using MiniMate.Weather.Models;

namespace MiniMate.Weather.Contracts
{
    public interface IWeatherService
    {
        Task<WeatherData?> GetCurrentWeatherAsync(double latitude, double longitude);
        Task<LocationData[]> SearchLocationAsync(string query);
        Task<HourlyForecastData[]> GetHourlyForecastAsync(double latitude, double longitude);
        Task<DailyForecastData[]> GetDailyForecastAsync(double latitude, double longitude);
    }
}
