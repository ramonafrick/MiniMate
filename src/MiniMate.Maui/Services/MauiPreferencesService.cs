using Microsoft.JSInterop;

namespace MiniMate.Maui.Services
{
    /// <summary>
    /// MAUI-specific preferences service that provides JavaScript interop
    /// for preferences storage using native MAUI Preferences API
    /// </summary>
    public class MauiPreferencesService
    {
        [JSInvokable("GetPreference")]
        public static string? GetPreference(string key)
        {
            return Preferences.Get(key, null);
        }

        [JSInvokable("SetPreference")]
        public static void SetPreference(string key, string value)
        {
            Preferences.Set(key, value);
        }
    }
}
