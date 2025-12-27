using System.Text.Json.Serialization;
using MiniMate.Modules.Location.Domain;

namespace MiniMate.Modules.Location.Application.Models
{
    public record LocationResponse(
    [property: JsonPropertyName("results")] LocationData[]? Results
);
}
