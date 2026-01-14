using Microsoft.Extensions.DependencyInjection;
using MiniMate.Modules.Profile.Application.Contracts;
using MiniMate.Modules.Profile.Application.Services;

namespace MiniMate.Modules.Profile
{
    public static class ProfileModuleExtensions
    {
        public static IServiceCollection AddProfileModule(this IServiceCollection services)
        {
            // Register ProfileStateService as Singleton so it persists across navigation
            services.AddSingleton<ProfileStateService>();

            // Register CultureStateService as Singleton for app-wide culture management
            services.AddSingleton<CultureStateService>();

            services.AddScoped<IProfileService, ProfileService>();
            services.AddLocalization();

            return services;
        }
    }
}
