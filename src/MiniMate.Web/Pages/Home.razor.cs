namespace MiniMate.Pages
{
    public partial class Home
    {
        private string UserName = "Max";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var profile = await ProfileService.GetProfileAsync();
            UserName = string.IsNullOrWhiteSpace(profile.Name) ? "Max" : profile.Name;
        }
    }
}
