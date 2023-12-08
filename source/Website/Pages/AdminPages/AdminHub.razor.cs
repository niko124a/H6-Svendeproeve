
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace Website.Pages.AdminPages
{
    public partial class AdminHub
    {
        [Inject] public AuthenticationStateProvider CustomAuthenticationStateProvider { get; set; }

        private AuthenticationState authState;
        private string userRole = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            authState = await CustomAuthenticationStateProvider.GetAuthenticationStateAsync();

            string? role = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!string.IsNullOrEmpty(role))
                userRole = role;
        }
    }
}
