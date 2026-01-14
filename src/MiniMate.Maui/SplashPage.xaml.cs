namespace MiniMate.Maui
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Start the bouncing animation
            await AnimateLogo();

            // Navigate to main page after animation
            await Task.Delay(1500); // Show splash for 1.5 seconds total

            // Use the modern way to set the main page
            if (Application.Current?.Windows.Count > 0)
            {
                Application.Current.Windows[0].Page = new AppShell();
            }
        }

        private async Task AnimateLogo()
        {
            // Bounce animation loop (3 bounces)
            for (int i = 0; i < 3; i++)
            {
                // Bounce up
                await LogoImage.TranslateToAsync(0, -50, 400, Easing.CubicOut);
                // Bounce down
                await LogoImage.TranslateToAsync(0, 0, 400, Easing.BounceOut);
            }
        }
    }
}
