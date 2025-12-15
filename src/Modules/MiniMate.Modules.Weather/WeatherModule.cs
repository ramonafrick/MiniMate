using Microsoft.Extensions.DependencyInjection;
using MiniMate.Modules.Weather.Application.Contracts;
using MiniMate.Modules.Weather.Application.Services;
using MiniMate.Shared.Kernel.Abstractions;

namespace MiniMate.Modules.Weather;

/// <summary>
/// Weather module registration implementing the IModule interface.
/// Provides extension method for easy service registration.
/// </summary>
public class WeatherModule : IModule
{
    public string ModuleName => "Weather";

    public void RegisterServices(IServiceCollection services)
    {
        // Register Weather Service
        services.AddScoped<IWeatherService, WeatherService>();
    }
}

/// <summary>
/// Extension methods for registering the Weather module.
/// </summary>
public static class WeatherModuleExtensions
{
    /// <summary>
    /// Adds the Weather module services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddWeatherModule(this IServiceCollection services)
    {
        var module = new WeatherModule();
        module.RegisterServices(services);
        return services;
    }
}
