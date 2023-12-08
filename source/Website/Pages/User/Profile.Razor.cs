
using Azure;
using Azure.Core;
using Blazored.LocalStorage;
using Common.Entities;
using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using WebAPI.Models.Car;
using WebAPI.Models.Reservation;
using WebAPI.Models.User;
using Website.Authentication;
using Website.Helpers;
using Website.Models;
using Website.Pages.Car;

namespace Website.Pages.User
{
    public partial class Profile
    {
        [Inject] public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] public IConfiguration Configuration { get; set; }
        [Inject] public ILocalStorageService LocalStorageService { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] public AuthenticationStateProvider CustomAuthenticationStateProvider { get; set; }
        [Inject] public JsonSerializerOptionsWrapper JsonSerializerOptionsWrapper { get; set; }
        [Inject] public ApiResponseHelper ApiResponseHelper { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        private AuthenticationState authState;
        private Common.Entities.User user = new();
        private UpdateUserDto updateUserDto = new();
        private CarDto carDto = new();

        private List<Common.Entities.Car> userCars = new();
        private List<Common.Entities.Reservation> userActiveReservations = new();

        private bool synsbasenApiIsValid = false;


        protected override async Task OnInitializedAsync()
        {
            authState = await CustomAuthenticationStateProvider.GetAuthenticationStateAsync();

            // Get user
            var httpClient = HttpClientFactory.CreateClient("WebApiClient");

            int id = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
            var authToken = await LocalStorageService.GetItemAsync<string>("JwtToken");

            try
            {
                using (var userRequest = new HttpRequestMessage(HttpMethod.Get, $"api/users/{id}"))
                {

                    userRequest.Headers.Add("Authorization", $"Bearer {authToken}");
                    userRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                    var userRequestResponse = await httpClient.SendAsync(userRequest, HttpCompletionOption.ResponseHeadersRead);

                    var result = await ApiResponseHelper.HandleApiResponseGetSingleAsync<Common.Entities.User>(userRequestResponse, true);
                    if (result != null && result != default)
                    {
                        user = result;

                        if (userRequestResponse.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            updateUserDto.FirstName = user.FirstName;
                            updateUserDto.LastName = user.LastName;
                            updateUserDto.Email = user.Email;
                            updateUserDto.PhoneNumber = user.PhoneNumber;
                        }
                    }
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to get the user was canceled with the message: {ocException.Message}");
            }

            // Get users cars
            httpClient = HttpClientFactory.CreateClient("WebApiClient");

            try
            {
                using (var carsRequest = new HttpRequestMessage(HttpMethod.Get, $"api/cars/user/{user.Id}"))
                {
                    carsRequest.Headers.Add("Authorization", $"Bearer {authToken}");
                    carsRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var carsRequestResponse = await httpClient.SendAsync(carsRequest, HttpCompletionOption.ResponseHeadersRead);

                    var result = await ApiResponseHelper.HandleApiResponseGetMultipleAsync<Common.Entities.Car>(carsRequestResponse, true);
                    if (result != null)
                        userCars = result;
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to get the users cars was canceled with the message: {ocException.Message}");
            }

            // Get users active reservations
            httpClient = HttpClientFactory.CreateClient("WebApiClient");

            try
            {
                using (var activeReservationsRequest = new HttpRequestMessage(HttpMethod.Get, $"api/reservations/user/{user.Id}/active"))
                {
                    activeReservationsRequest.Headers.Add("Authorization", $"Bearer {authToken}");
                    activeReservationsRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var activeReservationsRequestResponse = await httpClient.SendAsync(activeReservationsRequest, HttpCompletionOption.ResponseHeadersRead);

                    var result = await ApiResponseHelper.HandleApiResponseGetMultipleAsync<Common.Entities.Reservation>(activeReservationsRequestResponse, true);
                    if (result != null)
                        userActiveReservations = result;
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to get the users active reservations was canceled with the message: {ocException.Message}");
            }
        }


        bool cannotEditInfo = true;
        public async Task UpdateInfoSubmit()
        {
            cannotEditInfo = true;

            var httpClient = HttpClientFactory.CreateClient("WebApiClient");

            int id = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
            var authToken = await LocalStorageService.GetItemAsync<string>("JwtToken");

            try
            {
                using (var memoryContentStream = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(memoryContentStream, updateUserDto);
                    memoryContentStream.Seek(0, SeekOrigin.Begin); // sets the stream back to the beginning.

                    using (var request = new HttpRequestMessage(HttpMethod.Put, $"api/users/{id}"))
                    {
                        request.Headers.Add("Authorization", $"Bearer {authToken}");
                        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        using (var streamContent = new StreamContent(memoryContentStream))
                        {
                            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            request.Content = streamContent;

                            var response = await httpClient.SendAsync(request);
                            await ApiResponseHelper.HandleApiResponsePutAsync<Common.Entities.User>(response);
                        }
                    }
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to update user information was canceled with the message: {ocException.Message}");
            }
        }

        string registrationDmr = string.Empty;
        bool haveCustomRegistration = false;
        public async Task FindCarInDmr()
        {
            var synsbasenAuthToken = Configuration.GetValue<string>("SynsbasenApi:Token");
            var httpClient = HttpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(Configuration.GetValue<string>("SynsbasenApi:Address"));

            try
            {
                using (var synsbasenRequest = new HttpRequestMessage(HttpMethod.Get, $"vehicles/registration/{registrationDmr.ToUpper()}"))
                {
                    synsbasenRequest.Headers.Add("Authorization", $"Bearer {synsbasenAuthToken}");

                    var response = await httpClient.SendAsync(synsbasenRequest, HttpCompletionOption.ResponseHeadersRead);
                    var result = await ApiResponseHelper.HandleApiResponseGetSingleAsync<SynsbasenApiCarDto>(response, false);
                    if (result != null && result != default)
                    {
                        carDto.UserId = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
                        carDto.Registration = result.data.registration;
                        carDto.Vin = result.data.vin;
                        carDto.Status = result.data.status;
                        carDto.Kind = result.data.kind;
                        carDto.Usage = result.data.usage;
                        carDto.Category = result.data.category;
                        carDto.ModelYear = result.data.model_year;
                        carDto.Brand = result.data.brand;
                        carDto.Model = result.data.model;
                        carDto.Variant = result.data.variant;
                        carDto.FuelType = result.data.fuel_type;
                        carDto.ExtraEquipment = result.data.extra_equipment;
                    }
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to find a car in dmr was canceled with the message: {ocException.Message}");
            }


            DialogOptions addCarDialogOptions = new DialogOptions()
            {
                Position = DialogPosition.Center
            };
            var addCarDialogParameters = new DialogParameters();
            addCarDialogParameters.Add("CarDto", carDto);
            var dialogResult = DialogService.Show<AddCarDialog>("Tilføj bil", addCarDialogParameters, addCarDialogOptions).Result.ConfigureAwait(true);
            var addCarDialogResult = await dialogResult;
            if (!addCarDialogResult.Cancelled)
            {
                httpClient = HttpClientFactory.CreateClient("WebApiClient");
                var authToken = await LocalStorageService.GetItemAsync<string>("JwtToken");

                try
                {
                    using (var addCarRequest = new HttpRequestMessage(HttpMethod.Post, $"api/cars?isCustom={haveCustomRegistration}"))
                    {
                        addCarRequest.Headers.Add("Authorization", $"Bearer {authToken}");
                        addCarRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        using (var memoryContentStream = new MemoryStream())
                        {
                            await JsonSerializer.SerializeAsync(memoryContentStream, carDto);
                            memoryContentStream.Seek(0, SeekOrigin.Begin);

                            using (var streamContent = new StreamContent(memoryContentStream))
                            {
                                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                                addCarRequest.Content = streamContent;

                                var addCarResponse = await httpClient.SendAsync(addCarRequest);
                                var result = await ApiResponseHelper.HandleApiResponsePostAsync<Common.Entities.Car>(addCarResponse, false);
                                if (addCarResponse.StatusCode == System.Net.HttpStatusCode.Created)
                                {
                                    Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                                    Snackbar.Add($"Bil tilføjet.", Severity.Success);
                                    NavigationManager.NavigateTo("/profile", true);
                                }
                            }
                        }
                    }
                }
                catch (OperationCanceledException ocException)
                {
                    Console.WriteLine($"The operation to add a car to the user was canceled with the message: {ocException.Message}");
                }

            }

            registrationDmr = string.Empty;
            haveCustomRegistration = false;
        }

        public async Task RemoveCarAsync(Common.Entities.Car car)
        {
            var activeReservationsForCar = userActiveReservations.Where(ar => ar.Car.Id == car.Id);
            if (activeReservationsForCar.Count() != 0)
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                Snackbar.Add($"Du har stadig aktive reservationer med denne bil. Venligst annuller dine reservationer inden du fjerner bilen.", Severity.Warning);
                return;
            }

            var authToken = await LocalStorageService.GetItemAsync<string>("JwtToken");
            var userId = authState.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var httpClient = HttpClientFactory.CreateClient("WebApiClient");

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Delete, $"api/cars/{car.Id}"))
                {
                    request.Headers.Add("Authorization", $"Bearer {authToken}");
                    request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await httpClient.SendAsync(request);
                    await ApiResponseHelper.HandleApiResponseDeleteAsync<Common.Entities.Car>(response);

                    userCars.Remove(car);
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to remove a car from the user was canceled with the message: {ocException.Message}");
            }
        }

        public async Task CancelReservationAsync(Common.Entities.Reservation reservation)
        {
            var authToken = await LocalStorageService.GetItemAsync<string>("JwtToken");
            var userId = authState.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var httpClient = HttpClientFactory.CreateClient("WebApiClient");

            var reservationDto = new ReservationDto() 
            {
                UserId = reservation.User.Id,
                ReservationTypeId = reservation.Type.Id,
                StartDate = reservation.StartDate,
                Note = reservation.Note,
                CarId = reservation.Car.Id,
                Status = Common.Enums.Entitity.ReservationStatus.Annulleret.ToString()
            };

            try
            {
                using (var memoryContentStream = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(memoryContentStream, reservationDto);
                    memoryContentStream.Seek(0, SeekOrigin.Begin); // sets the stream back to the beginning.

                    using (var request = new HttpRequestMessage(HttpMethod.Put, $"api/reservations/{reservation.Id}"))
                    {
                        request.Headers.Add("Authorization", $"Bearer {authToken}");
                        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        using (var streamContent = new StreamContent(memoryContentStream))
                        {
                            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            request.Content = streamContent;

                            var response = await httpClient.SendAsync(request);
                            await ApiResponseHelper.HandleApiResponsePutAsync<Common.Entities.User>(response);

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                userActiveReservations.Remove(reservation);
                        }
                    }
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"The operation to remove a car from the user was canceled with the message: {ocException.Message}");
            }
        }
    }
}
