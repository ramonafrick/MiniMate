using Microsoft.Extensions.DependencyInjection;

namespace MiniMate.Modules.Calendar
{
    public static class CalendarModuleExtensions
    {
        public static IServiceCollection AddCalendarModule(this IServiceCollection services)
        {
            services.AddLocalization();

            return services;
        }
    }
}
