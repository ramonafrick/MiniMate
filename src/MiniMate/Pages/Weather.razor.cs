using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MiniMate.Modules.Weather.Resources;

namespace MiniMate.Pages
{
    public partial class Weather
    {
        #region Properties
        [Inject] protected IStringLocalizer<WeatherResources> Localizer { get; set; } = null!;
        #endregion
    }
}
