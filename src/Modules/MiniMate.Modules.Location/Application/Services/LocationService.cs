using MiniMate.Modules.Location.Application.Contracts;
using MiniMate.Modules.Location.Application.Models;
using MiniMate.Modules.Location.Domain;
using System.Globalization;
using System.Text.Json;

namespace MiniMate.Modules.Location.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;
        private const string GeocodingUrl = "https://geocoding-api.open-meteo.com/v1";

        public LocationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

        public async Task<string> GetLocationNameFromCoordinatesAsync(double latitude, double longitude)
        {
            // Use Nominatim (OpenStreetMap) for reverse geocoding
            // API Documentation: https://nominatim.openstreetmap.org/release-docs/latest/api/Reverse/
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
                        : nominatimResponse.DisplayName ?? "Unknown Location";
                }

                return "Unknown Location";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching location name: {ex.Message}");
                return "My Location";
            }
        }
    }
}
