using Microsoft.Extensions.Logging;
using MiniMate.Modules.Home;
using MiniMate.Modules.Location;
using MiniMate.Modules.Weather;
using MiniMate.Modules.Clothing;
using MiniMate.Modules.Calendar;
using MiniMate.Modules.Profile;

namespace MiniMate.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            // HttpClient for API calls
            builder.Services.AddScoped(sp => new HttpClient());

            // Register Localization
            builder.Services.AddLocalization();

            // Register Home Module
            builder.Services.AddHomeModule();

            // Register Location Module (must be registered before modules that depend on it)
            builder.Services.AddLocationModule();

            // Register Weather Module
            builder.Services.AddWeatherModule();

            // Register Clothing Module (depends on Weather Module)
            builder.Services.AddClothingModule();

            // Register Calendar Module
            builder.Services.AddCalendarModule();

            // Register Profile Module
            builder.Services.AddProfileModule();

            return builder.Build();
        }
    }
}
