using Microsoft.Extensions.DependencyInjection;
using MiniMate.Modules.Weather.Application.Contracts;
using MiniMate.Modules.Weather.Application.Services;

namespace MiniMate.Modules.Weather
{
    public static class WeatherModuleExtensions
    {
        /// <summary>
        /// Registers all Weather module services and dependencies
        /// </summary>
        public static IServiceCollection AddWeatherModule(this IServiceCollection services)
        {
            // Register HttpClient and WeatherService together
            // This automatically registers IWeatherService as well
            services.AddHttpClient<IWeatherService, WeatherService>();

            // Register localization
            services.AddLocalization();

            return services;
        }
    }
}
