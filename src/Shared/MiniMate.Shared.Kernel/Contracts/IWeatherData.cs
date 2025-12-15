namespace MiniMate.Shared.Kernel.Contracts;

/// <summary>
/// Shared contract for weather data used across modules.
/// This interface allows modules like Clothing to depend on weather information
/// without having a direct dependency on the Weather module.
/// </summary>
public interface IWeatherData
{
    /// <summary>
    /// Gets the current weather conditions.
    /// </summary>
    ICurrentWeather Current { get; }
}

/// <summary>
/// Represents current weather conditions.
/// </summary>
public interface ICurrentWeather
{
    /// <summary>
    /// Temperature in degrees Celsius.
    /// </summary>
    double Temperature { get; }

    /// <summary>
    /// Apparent temperature (feels like) in degrees Celsius.
    /// </summary>
    double ApparentTemperature { get; }

    /// <summary>
    /// Rainfall in millimeters.
    /// </summary>
    double Rain { get; }

    /// <summary>
    /// Snowfall in centimeters.
    /// </summary>
    double Snowfall { get; }

    /// <summary>
    /// Wind speed in km/h.
    /// </summary>
    double WindSpeed { get; }

    /// <summary>
    /// WMO Weather interpretation code.
    /// See: https://open-meteo.com/en/docs
    /// </summary>
    int WeatherCode { get; }
}
