using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MiniMate;
using MiniMate.Modules.Location;
using MiniMate.Modules.Weather;
using MiniMate.Clothing.Contracts;
using MiniMate.Clothing.Services;
using MiniMate.Profile.Contracts;
using MiniMate.Profile.Services;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register Localization
builder.Services.AddLocalization();

// Register Location Module (must be registered before modules that depend on it)
builder.Services.AddLocationModule();

// Register Weather Module
builder.Services.AddWeatherModule();

// Register Clothing Service
builder.Services.AddScoped<IClothingService, ClothingService>();

// Register Profile Service
builder.Services.AddScoped<IProfileService, ProfileService>();

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
