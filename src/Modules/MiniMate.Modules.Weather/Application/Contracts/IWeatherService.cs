using MiniMate.Modules.Weather.Domain.Entities;
using MiniMate.Modules.Weather.Infrastructure.Models;

namespace MiniMate.Modules.Weather.Application.Contracts;

/// <summary>
/// Service for retrieving weather data and location information.
/// Orchestrates weather data retrieval, location search, and forecasts.
/// </summary>
public interface IWeatherService
{
    /// <summary>
    /// Gets current weather data for the specified coordinates.
    /// </summary>
    Task<WeatherData?> GetCurrentWeatherAsync(double latitude, double longitude);

    /// <summary>
    /// Searches for locations matching the query string.
    /// </summary>
    Task<LocationData[]> SearchLocationAsync(string query);

    /// <summary>
    /// Gets hourly weather forecast for the specified coordinates.
    /// </summary>
    Task<HourlyForecastData[]> GetHourlyForecastAsync(double latitude, double longitude);

    /// <summary>
    /// Gets daily weather forecast for the specified coordinates.
    /// </summary>
    Task<DailyForecastData[]> GetDailyForecastAsync(double latitude, double longitude);

    /// <summary>
    /// Gets the location name from geographic coordinates using reverse geocoding.
    /// </summary>
    Task<string> GetLocationNameFromCoordinatesAsync(double latitude, double longitude);
}
