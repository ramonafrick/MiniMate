using Microsoft.JSInterop;

namespace MiniMate.Maui.Services
{
    /// <summary>
    /// MAUI-specific geolocation service that provides JavaScript interop
    /// for location services using native MAUI APIs
    /// </summary>
    public class MauiGeolocationService
    {
        [JSInvokable("GetCurrentPositionAsync")]
        public static async Task<GeolocationResult> GetCurrentPositionAsync()
        {
            try
            {
                // Request location permissions
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (status != PermissionStatus.Granted)
                {
                    throw new Exception("Location permission denied");
                }

                // Get current location using MAUI's Geolocation API
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var location = await Microsoft.Maui.Devices.Sensors.Geolocation.Default.GetLocationAsync(request);

                if (location == null)
                {
                    throw new Exception("Unable to get location");
                }

                return new GeolocationResult
                {
                    Coords = new Coordinates
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Accuracy = location.Accuracy,
                        Altitude = location.Altitude,
                        AltitudeAccuracy = location.Accuracy,
                        Heading = location.Course,
                        Speed = location.Speed
                    },
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting location: {ex.Message}");
                throw;
            }
        }

        [JSInvokable("IsGeolocationAvailableAsync")]
        public static Task<bool> IsGeolocationAvailableAsync()
        {
            return Task.FromResult(true);
        }
    }

    public class GeolocationResult
    {
        public Coordinates Coords { get; set; } = new();
        public long Timestamp { get; set; }
    }

    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Accuracy { get; set; }
        public double? Altitude { get; set; }
        public double? AltitudeAccuracy { get; set; }
        public double? Heading { get; set; }
        public double? Speed { get; set; }
    }
}
