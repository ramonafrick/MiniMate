using MiniMate.Clothing.Models;
using MiniMate.Modules.Weather.Domain;

namespace MiniMate.Clothing.Contracts
{
    /// <summary>
    /// Service for determining appropriate clothing recommendations based on weather conditions
    /// </summary>
    public interface IClothingService
    {
        /// <summary>
        /// Gets a clothing recommendation based on current weather data
        /// </summary>
        /// <param name="weatherData">Current weather data</param>
        /// <returns>Clothing recommendation with image path and descriptions</returns>
        ClothingRecommendation GetClothingRecommendation(WeatherData weatherData);
    }
}
