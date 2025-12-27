using Microsoft.Extensions.DependencyInjection;
using MiniMate.Modules.Location.Application.Contracts;
using MiniMate.Modules.Location.Application.Services;

namespace MiniMate.Modules.Location
{
    public static class LocationModuleExtensions
    {
        /// <summary>
        /// Registers all Location module services and dependencies
        /// </summary>
        public static IServiceCollection AddLocationModule(this IServiceCollection services)
        {
            // Register HttpClient and LocationService together
            // This automatically registers ILocationService as well
            services.AddHttpClient<ILocationService, LocationService>();

            // Register localization
            services.AddLocalization();

            return services;
        }
    }
}
