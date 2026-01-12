using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MiniMate.Component
{
    public partial class ThemeToggle : ComponentBase, IAsyncDisposable
    {
        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

        private string _currentTheme = "light";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadTheme();
        }

        private async Task LoadTheme()
        {
            try
            {
                _currentTheme = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "theme") ?? "light";
                await ApplyTheme(_currentTheme);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading theme: {ex.Message}");
            }
        }

        private async Task ToggleTheme()
        {
            _currentTheme = _currentTheme == "light" ? "dark" : "light";
            await ApplyTheme(_currentTheme);

            try
            {
                await JSRuntime.InvokeVoidAsync("localStorage.setItem", "theme", _currentTheme);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving theme: {ex.Message}");
            }
        }

        private async Task ApplyTheme(string theme)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("eval", $"document.documentElement.setAttribute('data-theme', '{theme}')");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying theme: {ex.Message}");
            }
        }

        private string GetThemeIcon()
        {
            return _currentTheme == "light" ? "🌙" : "☀️";
        }

        private string GetThemeTitle()
        {
            return _currentTheme == "light" ? "Dark Mode aktivieren" : "Light Mode aktivieren";
        }

        public async ValueTask DisposeAsync()
        {
            // Cleanup if needed
            await Task.CompletedTask;
        }
    }
}
