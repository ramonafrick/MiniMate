using Microsoft.Extensions.Localization;
using MiniMate.Modules.Profile.Resources;
using MiniMate.Modules.Profile.Application.Services;
using System.Globalization;

namespace MiniMate.Maui.Pages
{
    public partial class ProfilePage : ContentPage
    {
        private readonly IStringLocalizer<ProfileResources> _localizer;
        private readonly CultureStateService _cultureStateService;

        public ProfilePage(IStringLocalizer<ProfileResources> localizer, CultureStateService cultureStateService)
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
            Console.WriteLine($"ProfilePage: Culture changed to {newCulture.Name}, updating title");
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Title = _localizer["PageTitle"];
            Console.WriteLine($"ProfilePage: Title updated to '{Title}'");
        }
    }
}
