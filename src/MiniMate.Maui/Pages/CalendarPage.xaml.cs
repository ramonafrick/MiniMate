using Microsoft.Extensions.Localization;
using MiniMate.Modules.Calendar.Resources;
using MiniMate.Modules.Profile.Application.Services;
using System.Globalization;

namespace MiniMate.Maui.Pages
{
    public partial class CalendarPage : ContentPage
    {
        private readonly IStringLocalizer<CalendarResources> _localizer;
        private readonly CultureStateService _cultureStateService;

        public CalendarPage(IStringLocalizer<CalendarResources> localizer, CultureStateService cultureStateService)
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
            Console.WriteLine($"CalendarPage: Culture changed to {newCulture.Name}, updating title");
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Title = _localizer["PageTitle"];
            Console.WriteLine($"CalendarPage: Title updated to '{Title}'");
        }
    }
}
