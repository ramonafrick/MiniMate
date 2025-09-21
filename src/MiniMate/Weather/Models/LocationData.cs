using System.Text.Json.Serialization;

namespace MiniMate.Weather.Models
{
    public record LocationData(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("elevation")] double? Elevation,
    [property: JsonPropertyName("feature_code")] string? FeatureCode,
    [property: JsonPropertyName("country_code")] string? CountryCode,
    [property: JsonPropertyName("admin1")] string? Admin1,
    [property: JsonPropertyName("admin2")] string? Admin2,
    [property: JsonPropertyName("admin3")] string? Admin3,
    [property: JsonPropertyName("admin4")] string? Admin4,
    [property: JsonPropertyName("timezone")] string? Timezone,
    [property: JsonPropertyName("population")] int? Population,
    [property: JsonPropertyName("country_id")] int? CountryId,
    [property: JsonPropertyName("country")] string? Country,
    [property: JsonPropertyName("postcodes")] string[]? Postcodes
)
    {
        public string DisplayName =>
            $"{Name}{(Admin1 != null ? $", {Admin1}" : "")}{(Country != null ? $", {Country}" : "")}";
    }
}
