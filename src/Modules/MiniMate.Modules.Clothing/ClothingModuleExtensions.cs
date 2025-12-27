using Microsoft.Extensions.DependencyInjection;
using MiniMate.Modules.Clothing.Application.Contracts;
using MiniMate.Modules.Clothing.Application.Services;

namespace MiniMate.Modules.Clothing
{
    public static class ClothingModuleExtensions
    {
        public static IServiceCollection AddClothingModule(this IServiceCollection services)
        {
            services.AddScoped<IClothingService, ClothingService>();
            services.AddLocalization();

            return services;
        }
    }
}
