using Microsoft.Extensions.Localization;
using MiniMate.Modules.Home.Resources;
using MiniMate.Modules.Profile.Application.Services;
using System.Globalization;

namespace MiniMate.Maui.Pages
{
    public partial class HomePage : ContentPage
    {
        private readonly IStringLocalizer<HomeResources> _localizer;
        private readonly CultureStateService _cultureStateService;

        public HomePage(IStringLocalizer<HomeResources> localizer, CultureStateService cultureStateService)
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
            Console.WriteLine($"HomePage: Culture changed to {newCulture.Name}, updating title");
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Title = _localizer["PageTitle"];
            Console.WriteLine($"HomePage: Title updated to '{Title}'");
        }
    }
}
