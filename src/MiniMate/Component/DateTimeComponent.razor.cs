using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MiniMate.Modules.Weather.Domain;
using MiniMate.Modules.Weather.Resources;
using System.Globalization;

namespace MiniMate.Component
{
    public partial class DateTimeComponent : ComponentBase, IDisposable
    {
        #region Properties
        private System.Timers.Timer? _timer;

        [Inject] protected IStringLocalizer<WeatherResources> Localizer { get; set; } = null!;

        /// <summary>
        /// Weather data containing the location's local time
        /// </summary>
        [Parameter]
        public WeatherData? WeatherData { get; set; }

        /// <summary>
        /// Current local time for the selected location
        /// Updates every second
        /// </summary>
        protected DateTime CurrentTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets the current month and year for calendar display
        /// </summary>
        protected DateTime CurrentMonth => new(CurrentTime.Year, CurrentTime.Month, 1);

        /// <summary>
        /// Gets the current season based on month and hemisphere
        /// </summary>
        protected string CurrentSeason => GetSeason(CurrentTime.Month, WeatherData?.IsNorthernHemisphere ?? true);

        /// <summary>
        /// Gets the season emoji icon
        /// </summary>
        protected string SeasonIcon => GetSeasonIcon(CurrentTime.Month, WeatherData?.IsNorthernHemisphere ?? true);

        /// <summary>
        /// Gets the current language for localization
        /// </summary>
        protected string CurrentLanguage => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        #endregion

        #region Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();

            // Initialize current time
            UpdateCurrentTime();

            // Update time every second
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += (sender, e) => UpdateTime();
            _timer.AutoReset = true;
            _timer.Start();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            // Update current time when weather data changes
            UpdateCurrentTime();
        }

        private void UpdateCurrentTime()
        {
            if (WeatherData != null)
            {
                // WeatherData.LocalTime now returns the current time in the location's timezone
                CurrentTime = WeatherData.LocalTime;
            }
            else
            {
                CurrentTime = DateTime.Now;
            }
        }

        private void UpdateTime()
        {
            InvokeAsync(() =>
            {
                // Get fresh time from WeatherData (which calculates current time in location's timezone)
                UpdateCurrentTime();
                StateHasChanged();
            });
        }

        /// <summary>
        /// Gets all days in the current month for calendar display
        /// </summary>
        protected List<int?> GetCalendarDays()
        {
            var days = new List<int?>();
            var firstDay = new DateTime(CurrentTime.Year, CurrentTime.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            // Get day of week for first day (Monday = 1, Sunday = 7)
            var firstDayOfWeek = (int)firstDay.DayOfWeek;
            if (firstDayOfWeek == 0) firstDayOfWeek = 7; // Sunday = 7

            // Add empty cells for days before month starts
            for (int i = 1; i < firstDayOfWeek; i++)
            {
                days.Add(null);
            }

            // Add all days of the month
            for (int day = 1; day <= lastDay.Day; day++)
            {
                days.Add(day);
            }

            return days;
        }

        /// <summary>
        /// Checks if a given day is today
        /// </summary>
        protected bool IsToday(int? day)
        {
            return day.HasValue && day.Value == CurrentTime.Day;
        }

        /// <summary>
        /// Gets the season name based on month and hemisphere
        /// </summary>
        /// <param name="month">Current month (1-12)</param>
        /// <param name="isNorthernHemisphere">True for Northern Hemisphere, False for Southern</param>
        private string GetSeason(int month, bool isNorthernHemisphere)
        {
            // For Southern Hemisphere, seasons are reversed (shift by 6 months)
            if (!isNorthernHemisphere)
            {
                month = ((month + 6 - 1) % 12) + 1;
            }

            return month switch
            {
                12 or 1 or 2 => Localizer["Season_Winter"],
                3 or 4 or 5 => Localizer["Season_Spring"],
                6 or 7 or 8 => Localizer["Season_Summer"],
                9 or 10 or 11 => Localizer["Season_Autumn"],
                _ => "?"
            };
        }

        /// <summary>
        /// Gets the season emoji icon based on month and hemisphere
        /// </summary>
        /// <param name="month">Current month (1-12)</param>
        /// <param name="isNorthernHemisphere">True for Northern Hemisphere, False for Southern</param>
        private string GetSeasonIcon(int month, bool isNorthernHemisphere)
        {
            // For Southern Hemisphere, seasons are reversed (shift by 6 months)
            if (!isNorthernHemisphere)
            {
                month = ((month + 6 - 1) % 12) + 1;
            }

            return month switch
            {
                12 or 1 or 2 => "❄️",  // Winter
                3 or 4 or 5 => "🌸",    // Spring
                6 or 7 or 8 => "☀️",    // Summer
                9 or 10 or 11 => "🍂",  // Autumn
                _ => "🌍"
            };
        }

        /// <summary>
        /// Gets localized month name
        /// </summary>
        protected string GetMonthName()
        {
            return CurrentTime.ToString("MMMM yyyy", CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// Gets localized day names for calendar header
        /// </summary>
        protected string[] GetDayNames()
        {
            var culture = CultureInfo.CurrentUICulture;
            var dayNames = culture.DateTimeFormat.AbbreviatedDayNames;

            // Reorder to start with Monday
            return new[]
            {
                dayNames[1], // Mo
                dayNames[2], // Tu
                dayNames[3], // We
                dayNames[4], // Th
                dayNames[5], // Fr
                dayNames[6], // Sa
                dayNames[0]  // Su
            };
        }

        public void Dispose()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
        #endregion
    }
}
