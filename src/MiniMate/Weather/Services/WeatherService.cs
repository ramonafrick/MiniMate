using MiniMate.Weather.Contracts;
using MiniMate.Weather.Models;
using MiniMate.Weather.Resources;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;

namespace MiniMate.Weather.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IStringLocalizer<WeatherResources> _localizer;

        public WeatherService(HttpClient httpClient, IStringLocalizer<WeatherResources> localizer)
        {
            _httpClient = httpClient;
            _localizer = localizer;
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
                    new WeatherData(weatherResponse.Latitude, weatherResponse.Longitude, weatherResponse.Timezone, weatherResponse.TimezoneAbbreviation, weatherResponse.UtcOffsetSeconds, weatherResponse.Current, weatherResponse.CurrentUnits) : null;
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

        public async Task<HourlyForecastData[]> GetHourlyForecastAsync(double latitude, double longitude)
        {
            var hourlyParams = string.Join(",", [
                "temperature_2m",
                "precipitation_probability",
                "precipitation",
                "rain",
                "showers",
                "weather_code",
                "cloud_cover",
                "wind_speed_10m",
                "wind_direction_10m",
                "relative_humidity_2m",
                "is_day"
            ]);

            var url = $"{BaseUrl}/forecast?" +
                      $"latitude={latitude.ToString("F4", CultureInfo.InvariantCulture)}&longitude={longitude.ToString("F4", CultureInfo.InvariantCulture)}&" +
                      $"hourly={hourlyParams}&" +
                      $"timezone=auto&" +
                      $"forecast_days=2";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var forecastResponse = JsonSerializer.Deserialize<ForecastResponse>(json);

                if (forecastResponse?.Hourly == null)
                    return [];

                // Get current time and calculate next full hour
                var now = DateTime.Now;
                var nextHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1);
                var endTime = nextHour.AddHours(24);

                var hourlyData = new List<HourlyForecastData>();
                var hourly = forecastResponse.Hourly;

                for (int i = 0; i < hourly.Time.Length; i++)
                {
                    try
                    {
                        var time = DateTime.ParseExact(hourly.Time[i], "yyyy-MM-ddTHH:mm", null);

                        // Include next 24 hours starting from next full hour
                        if (time >= nextHour && time < endTime)
                        {
                            hourlyData.Add(new HourlyForecastData
                            {
                                Time = time,
                                Temperature = hourly.Temperature[i],
                                PrecipitationProbability = hourly.PrecipitationProbability[i],
                                Precipitation = hourly.Precipitation[i],
                                Rain = hourly.Rain[i],
                                Showers = hourly.Showers[i],
                                WeatherCode = hourly.WeatherCode[i],
                                CloudCover = hourly.CloudCover[i],
                                WindSpeed = hourly.WindSpeed[i],
                                WindDirection = hourly.WindDirection[i],
                                Humidity = hourly.Humidity[i],
                                IsDay = hourly.IsDay[i] == 1
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing hourly data at index {i}: {ex.Message}");
                    }
                }

                return [.. hourlyData];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching hourly forecast: {ex.Message}");
                return [];
            }
        }

        public async Task<DailyForecastData[]> GetDailyForecastAsync(double latitude, double longitude)
        {
            var dailyParams = string.Join(",", [
                "temperature_2m_max",
                "temperature_2m_min",
                "precipitation_probability_max",
                "precipitation_sum",
                "rain_sum",
                "weather_code",
                "sunrise",
                "sunset",
                "wind_speed_10m_max",
                "wind_gusts_10m_max"
            ]);

            var url = $"{BaseUrl}/forecast?" +
                      $"latitude={latitude.ToString("F4", CultureInfo.InvariantCulture)}&longitude={longitude.ToString("F4", CultureInfo.InvariantCulture)}&" +
                      $"daily={dailyParams}&" +
                      $"timezone=auto&" +
                      $"forecast_days=7";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var forecastResponse = JsonSerializer.Deserialize<DailyForecastResponse>(json);

                if (forecastResponse?.Daily == null)
                    return [];

                var dailyData = new List<DailyForecastData>();
                var daily = forecastResponse.Daily;

                for (int i = 0; i < daily.Time.Length; i++)
                {
                    try
                    {
                        var date = DateTime.ParseExact(daily.Time[i], "yyyy-MM-dd", null);

                        DateTime? sunrise = null;
                        DateTime? sunset = null;

                        try
                        {
                            if (!string.IsNullOrEmpty(daily.Sunrise[i]))
                                sunrise = DateTime.ParseExact(daily.Sunrise[i], "yyyy-MM-ddTHH:mm", null);
                            if (!string.IsNullOrEmpty(daily.Sunset[i]))
                                sunset = DateTime.ParseExact(daily.Sunset[i], "yyyy-MM-ddTHH:mm", null);
                        }
                        catch
                        {
                            // Ignore sunrise/sunset parsing errors
                        }

                        dailyData.Add(new DailyForecastData
                        {
                            Date = date,
                            TemperatureMax = daily.TemperatureMax[i],
                            TemperatureMin = daily.TemperatureMin[i],
                            PrecipitationProbability = daily.PrecipitationProbabilityMax[i],
                            PrecipitationSum = daily.PrecipitationSum[i],
                            RainSum = daily.RainSum[i],
                            WeatherCode = daily.WeatherCode[i],
                            Sunrise = sunrise,
                            Sunset = sunset,
                            WindSpeedMax = daily.WindSpeedMax[i],
                            WindGustsMax = daily.WindGustsMax[i]
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing daily data at index {i}: {ex.Message}");
                    }
                }

                return [.. dailyData];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching daily forecast: {ex.Message}");
                return [];
            }
        }

        public async Task<string> GetLocationNameFromCoordinatesAsync(double latitude, double longitude)
        {
            // Use Nominatim (OpenStreetMap) for reverse geocoding
            // API Documentation: https://nominatim.org/release-docs/latest/api/Reverse/
            var url = $"https://nominatim.openstreetmap.org/reverse?" +
                      $"lat={latitude.ToString("F6", CultureInfo.InvariantCulture)}&" +
                      $"lon={longitude.ToString("F6", CultureInfo.InvariantCulture)}&" +
                      $"format=json&" +
                      $"addressdetails=1&" +
                      $"accept-language=de";

            try
            {
                // Nominatim requires a User-Agent header
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("User-Agent", "MiniMate/1.0");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var nominatimResponse = JsonSerializer.Deserialize<NominatimResponse>(json);

                if (nominatimResponse?.Address != null)
                {
                    var address = nominatimResponse.Address;

                    // Build location name from available address components
                    // Priority: village/town/city/municipality, county/state, country
                    var locationParts = new List<string>();

                    var localName = address.Village ?? address.Town ?? address.City ?? address.Municipality;
                    if (!string.IsNullOrEmpty(localName))
                        locationParts.Add(localName);

                    var region = address.County ?? address.State;
                    if (!string.IsNullOrEmpty(region) && region != localName)
                        locationParts.Add(region);

                    if (!string.IsNullOrEmpty(address.Country))
                        locationParts.Add(address.Country);

                    return locationParts.Count > 0
                        ? string.Join(", ", locationParts)
                        : nominatimResponse.DisplayName ?? _localizer["UnknownLocation"];
                }

                return _localizer["UnknownLocation"];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching location name: {ex.Message}");
                return _localizer["MyLocationText"];
            }
        }
    }
}
