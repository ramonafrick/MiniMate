using Microsoft.Extensions.DependencyInjection;

namespace MiniMate.Shared.Kernel.Abstractions;

/// <summary>
/// Base interface for all feature modules in the MiniMate application.
/// Each module (Weather, Clothing, Profile, Calendar) implements this interface
/// to provide a standardized way of registering its services with dependency injection.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Gets the unique name of the module (e.g., "Weather", "Clothing").
    /// </summary>
    string ModuleName { get; }

    /// <summary>
    /// Registers all services, components, and dependencies for this module.
    /// This method is called during application startup to configure the module.
    /// </summary>
    /// <param name="services">The service collection to register services into.</param>
    void RegisterServices(IServiceCollection services);
}
