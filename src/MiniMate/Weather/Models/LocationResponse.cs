using System.Text.Json.Serialization;

namespace MiniMate.Weather.Models
{
    public record LocationResponse(
    [property: JsonPropertyName("results")] LocationData[]? Results
);
}
