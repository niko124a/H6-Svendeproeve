using Blazored.LocalStorage;
using Common.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Text.Json;
using WebAPI.Models.User;
using Website.Authentication;
using Website.Helpers;

namespace Website.Pages.User
{
    public partial class Login
    {
        [Inject] public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] public IConfiguration Configuration { get; set; }
        [Inject] public ILocalStorageService LocalStorageService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public JsonSerializerOptionsWrapper JsonSerializerOptionsWrapper { get; set; }
        [Inject] public ApiResponseHelper ApiResponseHelper { get; set; }

        private string username = string.Empty;
        private string password = string.Empty;

        public async Task LoginSubmit()
        {
            var httpClient = HttpClientFactory.CreateClient("WebApiClient");

            LoginDto loginDto = new LoginDto { Username = username, Password = password };

            try
            {
                using (var memoryContentStream = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(memoryContentStream, loginDto);
                    memoryContentStream.Seek(0, SeekOrigin.Begin); // sets the stream back to the beginning.

                    using (var request = new HttpRequestMessage(HttpMethod.Post, "api/authentication/authenticate"))
                    {
                        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        using (var streamContent = new StreamContent(memoryContentStream))
                        {
                            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            request.Content = streamContent;

                            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                            var result = await ApiResponseHelper.HandleApiResponsePostAsync<string>(response, true);
                            if (result == null)
                                result = string.Empty;

                            await LocalStorageService.SetItemAsync("JwtToken", result);
                        }
                    }
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to login the user was canceled with the message: {ocException.Message}");
            }
            
            NavigationManager.NavigateTo("/", true);
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
    }
}
