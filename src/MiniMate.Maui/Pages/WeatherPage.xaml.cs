using Microsoft.Extensions.Localization;
using MiniMate.Modules.Weather.Resources;
using MiniMate.Modules.Profile.Application.Services;
using System.Globalization;

namespace MiniMate.Maui.Pages
{
    public partial class WeatherPage : ContentPage
    {
        private readonly IStringLocalizer<WeatherResources> _localizer;
        private readonly CultureStateService _cultureStateService;

        public WeatherPage(IStringLocalizer<WeatherResources> localizer, CultureStateService cultureStateService)
        {
            InitializeComponent();
            _localizer = localizer;
            _cultureStateService = cultureStateService;

            // Set initial title
            UpdateTitle();

            // Subscribe to culture changes
            _cultureStateService.CultureChanged += OnCultureChanged;
        }

        private void OnCultureChanged(object? sender, CultureInfo newCulture)
        {
            Console.WriteLine($"WeatherPage: Culture changed to {newCulture.Name}, updating title");
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Title = _localizer["PageTitle"];
            Console.WriteLine($"WeatherPage: Title updated to '{Title}'");
        }
    }
}
