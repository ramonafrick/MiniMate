using MiniMate.Modules.Location.Domain;

namespace MiniMate.Modules.Location.Application.Contracts
{
    public interface ILocationService
    {
        /// <summary>
        /// Search for locations by name
        /// </summary>
        /// <param name="query">Search query (e.g. city name)</param>
        /// <returns>Array of matching locations</returns>
        Task<LocationData[]> SearchLocationAsync(string query);

        /// <summary>
        /// Get location name from coordinates using reverse geocoding
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <returns>Location name</returns>
        Task<string> GetLocationNameFromCoordinatesAsync(double latitude, double longitude);
    }
}
