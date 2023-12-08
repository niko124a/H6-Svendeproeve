using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Common.Helpers;
using Website.Helpers;
using Common.Entities;
using WebAPI.Models.User;
using MudBlazor;

namespace Website.Pages.AdminPages
{
    public partial class UsersAdmin
    {
        [Inject] public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] public IConfiguration Configuration { get; set; }
        [Inject] public ILocalStorageService LocalStorageService { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public JsonSerializerOptionsWrapper JsonSerializerOptionsWrapper { get; set; }
        [Inject] public ApiResponseHelper ApiResponseHelper { get; set; }

        private List<Common.Entities.User> users = new();
        private AdminCreateUserDto createUserDto = new AdminCreateUserDto();
        private string confirmPassword = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            // Get user
            var httpClient = HttpClientFactory.CreateClient("WebApiClient");
            var authToken = await LocalStorageService.GetItemAsync<string>("JwtToken");

            try
            {
                using (var usersRequest = new HttpRequestMessage(HttpMethod.Get, $"api/users"))
                {
                    usersRequest.Headers.Add("Authorization", $"Bearer {authToken}");
                    usersRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await httpClient.SendAsync(usersRequest, HttpCompletionOption.ResponseHeadersRead);
                    var result = await ApiResponseHelper.HandleApiResponseGetMultipleAsync<Common.Entities.User>(response, true);
                    if (result != null)
                        users = result;
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to get the user was canceled with the message: {ocException.Message}");
            }
        }

        private async Task DeleteUserAsync(Common.Entities.User user)
        {
            var userCars = new List<Common.Entities.User>();
            var userReservations = new List<Common.Entities.User>();
        }

        private async Task CreateUserAsync()
        {
            var httpClient = HttpClientFactory.CreateClient("WebApiClient");
            var authToken = Configuration.GetValue<string>("APIData:WebsiteApiToken");

            using (var memoryContentStream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(memoryContentStream, createUserDto);
                memoryContentStream.Seek(0, SeekOrigin.Begin);

                using (var request = new HttpRequestMessage(HttpMethod.Post, "api/users/admin"))
                {
                    request.Headers.Add("Authorization", $"Bearer {authToken}");
                    request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    using (var streamContent = new StreamContent(memoryContentStream))
                    {
                        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        request.Content = streamContent;

                        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                        var result = await ApiResponseHelper.HandleApiResponsePostAsync<Common.Entities.User>(response, true);
                        if (response.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            createUserDto = new();

                            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            Snackbar.Add($"{response.StatusCode}: Bruger oprettet", Severity.Success);
                        }



                    }
                }
            }
        }

        bool isShown = false;
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        private void ShowPassword()
        {
            if (isShown)
            {
                isShown = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                isShown = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

        private string MatchPassword(string args)
        {
            if (createUserDto.Password != confirmPassword)
                return "Adgangskoderne er ikke ens.";
            return null;
        }
    }
}
