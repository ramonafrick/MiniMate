using System.Text.Json.Serialization;

namespace MiniMate.Modules.Weather.Infrastructure.Models;

/// <summary>
/// Response model from the Open-Meteo geocoding API.
/// </summary>
public record LocationResponse(
    [property: JsonPropertyName("results")] LocationData[]? Results
);
