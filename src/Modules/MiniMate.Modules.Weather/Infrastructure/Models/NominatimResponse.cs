using System.Text.Json.Serialization;

namespace MiniMate.Modules.Weather.Infrastructure.Models;

/// <summary>
/// Response from Nominatim (OpenStreetMap) Reverse Geocoding API
/// </summary>
public class NominatimResponse
{
    [JsonPropertyName("address")]
    public NominatimAddress? Address { get; set; }

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }
}

public class NominatimAddress
{
    [JsonPropertyName("village")]
    public string? Village { get; set; }

    [JsonPropertyName("town")]
    public string? Town { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("municipality")]
    public string? Municipality { get; set; }

    [JsonPropertyName("county")]
    public string? County { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("country_code")]
    public string? CountryCode { get; set; }
}
