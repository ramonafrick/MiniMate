using Microsoft.Extensions.DependencyInjection;

namespace MiniMate.Modules.Home
{
    public static class HomeModuleExtensions
    {
        public static IServiceCollection AddHomeModule(this IServiceCollection services)
        {
            // Add localization support
            services.AddLocalization();

            return services;
        }
    }
}
