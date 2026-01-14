using System.Globalization;

namespace MiniMate.Modules.Profile.Application.Services
{
    /// <summary>
    /// Singleton service to manage and notify about culture/language changes across the application.
    /// This service persists across navigation and component lifecycle.
    /// </summary>
    public class CultureStateService
    {
        private CultureInfo _currentCulture = CultureInfo.CurrentUICulture;

        /// <summary>
        /// Event raised when the culture/language changes
        /// </summary>
        public event EventHandler<CultureInfo>? CultureChanged;

        /// <summary>
        /// Gets the current cached culture
        /// </summary>
        public CultureInfo CurrentCulture => _currentCulture;

        /// <summary>
        /// Updates the current culture and notifies all subscribers.
        /// Sets both CurrentCulture and CurrentUICulture for the entire app.
        /// </summary>
        public void ChangeCulture(CultureInfo newCulture)
        {
            if (_currentCulture.Name == newCulture.Name)
            {
                Console.WriteLine($"CultureStateService: Culture already set to {newCulture.Name}, skipping");
                return;
            }

            Console.WriteLine($"CultureStateService: Changing culture from {_currentCulture.Name} to {newCulture.Name}");

            _currentCulture = newCulture;

            // Set the culture for the entire application
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;
            CultureInfo.CurrentCulture = newCulture;
            CultureInfo.CurrentUICulture = newCulture;

            // Notify all subscribers
            Console.WriteLine($"CultureStateService: Notifying {CultureChanged?.GetInvocationList().Length ?? 0} subscribers");
            CultureChanged?.Invoke(this, newCulture);
        }

        /// <summary>
        /// Gets the current culture name (e.g., "de", "en")
        /// </summary>
        public string GetCultureName() => _currentCulture.TwoLetterISOLanguageName;
    }
}
