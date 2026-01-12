using System.Globalization;

namespace MiniMate.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Set culture from preferences or default to German
            var cultureName = Preferences.Get("BlazorCulture", "de");
            var culture = new CultureInfo(cultureName);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell()) { Title = "MiniMate" };
        }
    }
}
