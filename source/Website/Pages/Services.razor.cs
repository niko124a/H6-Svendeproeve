using Blazored.LocalStorage;
using Common.Entities;
using Common.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text.Json;
using Website.Data;
using Website.Helpers;

namespace Website.Pages
{
    public partial class Services
    {
        public List<ReservationType> ReservationTypes { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ReservationService ReservationService { get; set; }
        [Inject] public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] public IConfiguration Configuration { get; set; }
        [Inject] public JsonSerializerOptionsWrapper JsonSerializerOptionsWrapper { get; set; }
        [Inject] public ApiResponseHelper ApiResponseHelper { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ReservationTypes = new List<ReservationType>();
            var httpClient = HttpClientFactory.CreateClient("WebApiClient");
            var authToken = Configuration.GetValue<string>("APIData:WebsiteApiToken");

            using (var request = new HttpRequestMessage(HttpMethod.Get, "api/reservationtypes"))
            {
                request.Headers.Add("Authorization", $"Bearer {authToken}");
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var requestResponse = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                var result = await ApiResponseHelper.HandleApiResponseGetMultipleAsync<ReservationType>(requestResponse, true);

                if (result != null)
                    ReservationTypes = result;
            }
        }

        public void SelectServiceClickEvent(ReservationType reservationType)
        {
            ReservationService.ReservationDto.ReservationType = reservationType;
            NavigationManager.NavigateTo($"createreservation", true);
        }
    }
}
