using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MiniMate;
using MiniMate.Modules.Weather;
using MiniMate.Clothing.Contracts;
using MiniMate.Clothing.Services;
using MiniMate.Profile.Contracts;
using MiniMate.Profile.Services;
using MiniMate.Shared.Kernel.Contracts;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register Localization
builder.Services.AddLocalization();

// Register Modules
builder.Services.AddWeatherModule();

// Register Clothing Service
builder.Services.AddScoped<IClothingService, ClothingService>();

// Register Profile Service (for both local and shared interfaces)
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<MiniMate.Profile.Contracts.IProfileService>(sp => sp.GetRequiredService<ProfileService>());
builder.Services.AddScoped<MiniMate.Shared.Kernel.Contracts.IProfileService>(sp => sp.GetRequiredService<ProfileService>());

// Build the host first to get services
var host = builder.Build();

// Get culture from localStorage or use default
var js = host.Services.GetRequiredService<IJSRuntime>();
var result = await js.InvokeAsync<string>("blazorCulture.get");

// Set culture from localStorage or default to German
var cultureName = !string.IsNullOrEmpty(result) ? result : "de";
var culture = new CultureInfo(cultureName);
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();
