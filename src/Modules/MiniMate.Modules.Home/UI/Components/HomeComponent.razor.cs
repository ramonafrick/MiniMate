using Microsoft.AspNetCore.Components;
using MiniMate.Modules.Profile.Application.Contracts;
using MiniMate.Modules.Profile.Application.Services;
using MiniMate.Modules.Profile.Application.Models;

namespace MiniMate.Modules.Home.UI.Components
{
    public partial class HomeComponent : ComponentBase, IDisposable
    {
        #region Properties
        [Inject] protected IProfileService ProfileService { get; set; } = null!;
        [Inject] protected ProfileStateService ProfileStateService { get; set; } = null!;

        protected string UserName { get; set; } = "Max";
        #endregion

        #region Methods
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Console.WriteLine("HomeComponent: OnInitializedAsync called");

            // Subscribe to ProfileStateService (Singleton - persists across navigation)
            ProfileStateService.ProfileStateChanged += OnProfileStateChanged;

            await LoadProfile();
        }

        private async Task LoadProfile()
        {
            try
            {
                var profile = await ProfileService.GetProfileAsync();
                var newName = string.IsNullOrWhiteSpace(profile.Name) ? "Max" : profile.Name;

                // Only update if name has changed to avoid unnecessary renders
                if (UserName != newName)
                {
                    UserName = newName;
                    Console.WriteLine($"HomeComponent: Profile loaded - Name: {UserName}");
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HomeComponent: Error loading profile: {ex.Message}");
                // Default to "Max" if profile not available
            }
        }

        private void OnProfileStateChanged(object? sender, UserProfile profile)
        {
            // Update username when profile state changes (via Singleton service)
            Console.WriteLine($"HomeComponent: ProfileStateChanged event received - Name: {profile.Name}, Language: {profile.Language}");
            InvokeAsync(() =>
            {
                var newName = string.IsNullOrWhiteSpace(profile.Name) ? "Max" : profile.Name;
                if (UserName != newName)
                {
                    UserName = newName;
                    Console.WriteLine($"HomeComponent: Username updated to: {UserName}");
                }
                // Always trigger StateHasChanged to update localized strings when profile changes
                // This is important for language changes
                StateHasChanged();
            });
        }

        public void Dispose()
        {
            Console.WriteLine("HomeComponent: Disposing - unsubscribing from ProfileStateChanged");
            ProfileStateService.ProfileStateChanged -= OnProfileStateChanged;
        }
        #endregion
    }
}
