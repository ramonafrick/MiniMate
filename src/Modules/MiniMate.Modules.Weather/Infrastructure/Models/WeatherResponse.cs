using System.Text.Json.Serialization;
using MiniMate.Modules.Weather.Domain.ValueObjects;

namespace MiniMate.Modules.Weather.Infrastructure.Models;

/// <summary>
/// Response model from the Open-Meteo weather API.
/// </summary>
public record WeatherResponse(
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("timezone")] string? Timezone,
    [property: JsonPropertyName("timezone_abbreviation")] string? TimezoneAbbreviation,
    [property: JsonPropertyName("utc_offset_seconds")] int UtcOffsetSeconds,
    [property: JsonPropertyName("current")] CurrentWeather? Current,
    [property: JsonPropertyName("current_units")] CurrentUnits? CurrentUnits
);
