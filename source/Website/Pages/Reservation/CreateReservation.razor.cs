using Azure;
using Blazored.LocalStorage;
using Common.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Utilities;
using System.Text.Json;
using WebAPI.Models.Reservation;
using Website.Data;
using Website.Helpers;

namespace Website.Pages.Reservation
{
    public partial class CreateReservation
    {
        [Inject] public ReservationService ReservationService { get; set; }
        [Inject] public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] public IConfiguration Configuration { get; set; }
        [Inject] public ILocalStorageService LocalStorageService { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public AuthenticationStateProvider CustomAuthenticationStateProvider { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public JsonSerializerOptionsWrapper JsonSerializerOptionsWrapper { get; set; }
        [Inject] public ApiResponseHelper ApiResponseHelper { get; set; }

        private AuthenticationState authState;
        private List<string> availableTimes = new List<string>();
        private List<Common.Entities.Car> cars = new List<Common.Entities.Car>();
        private MudForm form = new MudForm();
        private bool isFormValid;
        private string[] formErrors = { };
        private DateTime? date = DateTime.Today;
        private string selectedCarRegistration;
        private bool reservationTypeSelected = false;
        private List<Common.Entities.Reservation> allActiveReservations = new List<Common.Entities.Reservation>();

        protected override async Task OnInitializedAsync()
        {
            authState = await CustomAuthenticationStateProvider.GetAuthenticationStateAsync();
            int userId = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
            ReservationService.ReservationDto.UserId = userId;

            if (ReservationService.ReservationDto.ReservationType != null)
                reservationTypeSelected = true;

            var authToken = await LocalStorageService.GetItemAsync<string>("JwtToken");
            var httpClient = HttpClientFactory.CreateClient("WebApiClient");

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, $"api/cars/user/{ReservationService.ReservationDto.UserId}"))
                {
                    request.Headers.Add("Authorization", $"Bearer {authToken}");
                    request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var httpResponseMessage = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                    var result = await ApiResponseHelper.HandleApiResponseGetMultipleAsync<Common.Entities.Car>(httpResponseMessage, true);
                    if (result != null)
                        cars = result;
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to get the users cars was canceled with the message: {ocException.Message}");
            }

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, $"api/reservations/active"))
                {
                    request.Headers.Add("Authorization", $"Bearer {authToken}");
                    request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var httpResponseMessage = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                    var result = await ApiResponseHelper.HandleApiResponseGetMultipleAsync<Common.Entities.Reservation>(httpResponseMessage, true);
                    if (result != null)
                        allActiveReservations = result;
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to get the users cars was canceled with the message: {ocException.Message}");
            }

            availableTimes = CalculateAvailableTimes();
        }

        public async Task FormSubmit()
        {
            if (ReservationService.ReservationDto.StartDate.TimeOfDay == default)
                return;

            ReservationService.ReservationDto.Car = cars.First(c => c.Registration == selectedCarRegistration);

            var httpClient = HttpClientFactory.CreateClient("WebApiClient");

            ReservationDto apiReservationDto = new ReservationDto
            {
                UserId = ReservationService.ReservationDto.UserId,
                ReservationTypeId = ReservationService.ReservationDto.ReservationType.Id,
                Note = ReservationService.ReservationDto.Note,
                StartDate = ReservationService.ReservationDto.StartDate.ToUniversalTime(),
                CarId = ReservationService.ReservationDto.Car.Id,
                Status = Common.Enums.Entitity.ReservationStatus.Aktiv.ToString()
            };

            var authToken = await LocalStorageService.GetItemAsync<string>("JwtToken");

            try
            {
                using (var memoryContentStream = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(memoryContentStream, apiReservationDto);
                    memoryContentStream.Seek(0, SeekOrigin.Begin); // sets the stream back to the beginning.


                    using (var request = new HttpRequestMessage(HttpMethod.Post, "api/reservations"))
                    {
                        request.Headers.Add("Authorization", $"Bearer {authToken}");
                        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        using (var streamContent = new StreamContent(memoryContentStream))
                        {
                            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            request.Content = streamContent;

                            var response = await httpClient.SendAsync(request);
                            var result = await ApiResponseHelper.HandleApiResponsePostAsync<Common.Entities.Reservation>(response, true);

                            ReservationService = new();
                        }
                    }
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to add a new reservation was canceled with the message: {ocException.Message}");
            }

            NavigationManager.NavigateTo("/profile", true);
        }

        private List<string> CalculateAvailableTimes()
        {
            var availableTimesList = new List<string>();
            var selectedWeekdayName = date.Value.ToString("dddd");
            if (selectedWeekdayName == DayOfWeek.Saturday.ToString())
            {
                date = date.Value.AddDays(2);
                selectedWeekdayName = date.Value.ToString("dddd");
            }
            else if (selectedWeekdayName == DayOfWeek.Sunday.ToString())
            {
                date = date.Value.AddDays(1);
                selectedWeekdayName = date.Value.ToString("dddd");
            }
            var selectedWeekdayOpenHours = Configuration.GetValue<string>($"CompanyOpenHours:{selectedWeekdayName}");

            var openHoursArray = selectedWeekdayOpenHours.Split('-');

            int openHour = Convert.ToInt32(openHoursArray[0].Split(":")[0]);
            int closeHour = Convert.ToInt32(openHoursArray[1].Split(":")[0]);
            int minutes = 0;

            for (int i = openHour; i < closeHour; i++)
            {
                if (i == openHour)
                {
                    minutes = Convert.ToInt32(openHoursArray[0].Split(":")[1]);
                    if (minutes == 30)
                    {
                        var timeTaken = allActiveReservations.FirstOrDefault(x => x.StartDate.Date == date.Value.Date && x.StartDate.ToLocalTime().Hour == i && x.StartDate.Minute == minutes);
                        if (timeTaken == null)
                        {
                            availableTimesList.Add($"{i}:{minutes}");
                            minutes = 0;
                        }
                    }
                    else
                    {
                        var timeTaken = allActiveReservations.FirstOrDefault(x => x.StartDate.Date == date.Value.Date && x.StartDate.ToLocalTime().Hour == i && x.StartDate.Minute == 00);
                        if (timeTaken == null)
                        {
                            availableTimesList.Add($"{i}:00");
                        }

                        var timeTaken1 = allActiveReservations.FirstOrDefault(x => x.StartDate.Date == date.Value.Date && x.StartDate.ToLocalTime().Hour == i && x.StartDate.Minute == 30);
                        if (timeTaken1 == null)
                        {
                            availableTimesList.Add($"{i}:30");
                        }
                    }
                    continue;
                }

                var timeTaken2 = allActiveReservations.FirstOrDefault(x => x.StartDate.Date == date.Value.Date && x.StartDate.ToLocalTime().Hour == i && x.StartDate.Minute == 00);
                if (timeTaken2 == null)
                {
                    availableTimesList.Add($"{i}:00");
                }

                var timeTaken3 = allActiveReservations.FirstOrDefault(x => x.StartDate.Date == date.Value.Date && x.StartDate.ToLocalTime().Hour == i && x.StartDate.Minute == 30);
                if (timeTaken3 == null)
                {
                    availableTimesList.Add($"{i}:30");
                }
            }

            return availableTimesList;
        }

        private int[] CalculateTimeToAdd(Common.Entities.Reservation reservation, int hour, int minutes)
        {

            string timeToAdd = ((float)reservation.Type.DurationEstimate / 60).ToString("N1");
            string[] hoursAndMinutesToAdd = timeToAdd.ToString().Split(",");
            if (Convert.ToInt32(hoursAndMinutesToAdd[0]) > 0)
            {
                hour += Convert.ToInt32(hoursAndMinutesToAdd[0]);
            }
            if (Convert.ToInt32(hoursAndMinutesToAdd[1]) > 0)
            {
                minutes = 30;
            }

            int[] result = { hour, minutes };
            return result;
        }

        private void SelectTimeClickEvent(string time)
        {
            ReservationService.ReservationDto.StartDate = ConstructDateTime((DateTime)date, time);
        }

        private DateTime ConstructDateTime(DateTime date, string time)
        {
            var timeArray = time.Split(':');
            return new DateTime(date.Year, date.Month, date.Day, Convert.ToInt32(timeArray[0]), Convert.ToInt32(timeArray[1]), 0).ToUniversalTime();
        }

        private void DateChanged(string value)
        {
            DateTime? inputDate = DateTime.Parse(value);
            var selectedWeekdayName = inputDate.Value.ToString("dddd");
            if (selectedWeekdayName == "Saturday")
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                Snackbar.Add($"Det er ikke muligt at vælge lørdage.", Severity.Warning);
                return;
            }
            else if (selectedWeekdayName == "Sunday")
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                Snackbar.Add($"Det er ikke muligt at vælge søndage.", Severity.Warning);
                return;
            }

            date = inputDate;
            availableTimes = CalculateAvailableTimes();
        }
    }
}
