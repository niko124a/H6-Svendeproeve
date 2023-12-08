using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Security.Claims;
using Blazored.LocalStorage;

namespace Website.Shared
{
    public partial class MainLayout
    {
        private MudTheme Theme = new MudTheme();
        [Inject] public ILocalStorageService LocalStorageService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public AuthenticationStateProvider CustomAuthenticationStateProvider { get; set; }
        private AuthenticationState authState;
        private string userRole = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            authState = await CustomAuthenticationStateProvider.GetAuthenticationStateAsync();
            userRole = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        }

        private async Task Logout()
        {
            await LocalStorageService.RemoveItemAsync("JwtToken");
            NavigationManager.NavigateTo("/", true);
        }
    }
}
