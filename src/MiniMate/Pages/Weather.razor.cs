using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MiniMate.Weather.Resources;

namespace MiniMate.Pages
{
    public partial class Weather
    {
        #region Properties
        [Inject] protected IStringLocalizer<WeatherResources> Localizer { get; set; } = null!;
        #endregion
    }
}
