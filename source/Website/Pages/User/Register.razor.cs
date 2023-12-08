using Common.Entities;
using Common.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using System;
using System.Net.Http;
using System.Text.Json;
using WebAPI.Models.User;
using Website.Helpers;

namespace Website.Pages.User
{
    public partial class Register
    {
        [Inject] public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] public IConfiguration Configuration { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public JsonSerializerOptionsWrapper JsonSerializerOptionsWrapper { get; set; }
        [Inject] public ApiResponseHelper ApiResponseHelper { get; set; }

        CreateUserDto createUserDto = new CreateUserDto();

        public async Task RegisterSubmit()
        {
            var httpClient = HttpClientFactory.CreateClient("WebApiClient");
            var authToken = Configuration.GetValue<string>("APIData:WebsiteApiToken");

            using (var memoryContentStream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(memoryContentStream, createUserDto);
                memoryContentStream.Seek(0, SeekOrigin.Begin);

                using (var request = new HttpRequestMessage(HttpMethod.Post, "api/users"))
                {
                    request.Headers.Add("Authorization", $"Bearer {authToken}");
                    request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    using (var streamContent = new StreamContent(memoryContentStream))
                    {
                        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        request.Content = streamContent;

                        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                        var result = await ApiResponseHelper.HandleApiResponsePostAsync<Common.Entities.User>(response, true);
                    }
                }
            }

            NavigationManager.NavigateTo("/login", true);
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

        string secondPassword = string.Empty;
        private string MatchPassword(string args)
        {
            if (createUserDto.Password != secondPassword)
                return "Adgangskoderne er ikke ens.";
            return null;
        }
    }
}
