using MiniMate.Weather.Contracts;
using MiniMate.Weather.Models;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;

namespace MiniMate.Weather.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private const string BaseUrl = "https://api.open-meteo.com/v1";
        private const string GeocodingUrl = "https://geocoding-api.open-meteo.com/v1";

        public async Task<WeatherData?> GetCurrentWeatherAsync(double latitude, double longitude)
        {
            var currentParams = string.Join(",", [
                "temperature_2m",
            "relative_humidity_2m",
            "apparent_temperature",
            "is_day",
            "precipitation",
            "rain",
            "showers",
            "snowfall",
            "weather_code",
            "cloud_cover",
            "surface_pressure",
            "wind_speed_10m",
            "wind_direction_10m",
            "wind_gusts_10m",
            "uv_index",
            "visibility"
            ]);

            var url = $"{BaseUrl}/forecast?" +
                      $"latitude={latitude.ToString("F4", CultureInfo.InvariantCulture)}&longitude={longitude.ToString("F4", CultureInfo.InvariantCulture)}&" +
                      $"current={currentParams}&" +
                      $"timezone=auto&" +
                      $"forecast_days=1";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(json);

                return weatherResponse?.Current != null ?
                    new WeatherData(weatherResponse.Current, weatherResponse.CurrentUnits) : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching weather data: {ex.Message}");
                return null;
            }
        }

        public async Task<LocationData[]> SearchLocationAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
                return [];

            var url = $"{GeocodingUrl}/search?name={Uri.EscapeDataString(query)}&count=10&language=de&format=json";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var locationResponse = JsonSerializer.Deserialize<LocationResponse>(json);

                return locationResponse?.Results ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching locations: {ex.Message}");
                return [];
            }
        }
    }
}
