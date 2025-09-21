using MiniMate.Weather.Models;

namespace MiniMate.Weather.Contracts
{
    public interface IWeatherService
    {
        Task<WeatherData?> GetCurrentWeatherAsync(double latitude, double longitude);
        Task<LocationData[]> SearchLocationAsync(string query);
    }
}
