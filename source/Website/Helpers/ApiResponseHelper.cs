using Common.Helpers;
using System.Text.Json;
using MudBlazor;

namespace Website.Helpers
{
    public class ApiResponseHelper
    {
        private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;
        private readonly ISnackbar _snackbar;
        public ApiResponseHelper(JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper, ISnackbar snackbar)
        {
            _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper;
            _snackbar = snackbar;
        }

        public async Task<List<T>?> HandleApiResponseGetMultipleAsync<T>(HttpResponseMessage response, bool useJsonSerializerOptions)
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    List<T>? responseList = null;
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    if (useJsonSerializerOptions)
                        responseList = await JsonSerializer.DeserializeAsync<List<T>>(responseStream, _jsonSerializerOptionsWrapper.Options);
                    else
                        responseList = await JsonSerializer.DeserializeAsync<List<T>>(responseStream);

                    if (responseList != null)
                        return responseList;
                    break;

                case System.Net.HttpStatusCode.NoContent:
                    return null;

                case System.Net.HttpStatusCode.BadRequest:
                    var badRequestStream = await response.Content.ReadAsStreamAsync();
                    var badRequestMessage = await JsonSerializer.DeserializeAsync<string>(badRequestStream);

                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: {badRequestMessage}", Severity.Warning);
                    return null;

                case System.Net.HttpStatusCode.NotFound:
                    var notFoundStream = await response.Content.ReadAsStreamAsync();
                    var notFoundMessage = await JsonSerializer.DeserializeAsync<string>(notFoundStream);

                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: {notFoundMessage}", Severity.Error);
                    return null;

                default:
                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: Something went wrong on the server.", Severity.Error);
                    return null;
            }
            return null;
        }

        public async Task<T?> HandleApiResponseGetSingleAsync<T>(HttpResponseMessage response, bool useJsonSerializerOptions)
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    T? responseEntity = default;
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    if (useJsonSerializerOptions)
                        responseEntity = await JsonSerializer.DeserializeAsync<T>(responseStream, _jsonSerializerOptionsWrapper.Options);
                    else
                        responseEntity = await JsonSerializer.DeserializeAsync<T>(responseStream);

                    if (responseEntity != null)
                        return responseEntity;
                    break;

                case System.Net.HttpStatusCode.NoContent:
                    return default;

                case System.Net.HttpStatusCode.BadRequest:
                    var badRequestStream = await response.Content.ReadAsStreamAsync();
                    var badRequestMessage = await JsonSerializer.DeserializeAsync<string>(badRequestStream);

                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: {badRequestMessage}", Severity.Warning);
                    return default;

                case System.Net.HttpStatusCode.NotFound:
                    var notFoundStream = await response.Content.ReadAsStreamAsync();
                    var notFoundMessage = await JsonSerializer.DeserializeAsync<string>(notFoundStream);

                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: {notFoundMessage}", Severity.Error);
                    return default;

                default:
                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: Something went wrong on the server.", Severity.Error);
                    return default;
            }
            return default;
        }

        public async Task<T?> HandleApiResponsePostAsync<T>(HttpResponseMessage response, bool useJsonSerializerOptions)
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    T? createdResponseEntity = default;
                    var createdResponseStream = await response.Content.ReadAsStreamAsync();
                    if (useJsonSerializerOptions)
                        createdResponseEntity = await JsonSerializer.DeserializeAsync<T>(createdResponseStream, _jsonSerializerOptionsWrapper.Options);
                    else
                        createdResponseEntity = await JsonSerializer.DeserializeAsync<T>(createdResponseStream);

                    if (createdResponseEntity != null)
                        return createdResponseEntity;
                    break;

                case System.Net.HttpStatusCode.OK:
                    T? okResponseEntity = default;
                    var okResponseStream = await response.Content.ReadAsStreamAsync();
                    if (useJsonSerializerOptions)
                        okResponseEntity = await JsonSerializer.DeserializeAsync<T>(okResponseStream, _jsonSerializerOptionsWrapper.Options);
                    else
                        okResponseEntity = await JsonSerializer.DeserializeAsync<T>(okResponseStream);

                    if (okResponseEntity != null)
                        return okResponseEntity;
                    break;

                case System.Net.HttpStatusCode.BadRequest:
                    var badRequestStream = await response.Content.ReadAsStreamAsync();
                    var badRequestMessage = await JsonSerializer.DeserializeAsync<string>(badRequestStream);

                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: {badRequestMessage}", Severity.Warning);
                    return default;

                case System.Net.HttpStatusCode.NotFound:
                    var notFoundStream = await response.Content.ReadAsStreamAsync();
                    var notFoundMessage = await JsonSerializer.DeserializeAsync<string>(notFoundStream);

                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: {notFoundMessage}", Severity.Error);
                    return default;

                default:
                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: Something went wrong on the server.", Severity.Error);
                    return default;
            }
            return default;
        }

        public async Task<T?> HandleApiResponsePutAsync<T>(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: Update Success.", Severity.Success);
                    return default;

                case System.Net.HttpStatusCode.BadRequest:
                    var badRequestStream = await response.Content.ReadAsStreamAsync();
                    var badRequestMessage = await JsonSerializer.DeserializeAsync<string>(badRequestStream);

                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: {badRequestMessage}", Severity.Warning);
                    return default;

                case System.Net.HttpStatusCode.NotFound:
                    var notFoundStream = await response.Content.ReadAsStreamAsync();
                    var notFoundMessage = await JsonSerializer.DeserializeAsync<string>(notFoundStream);

                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: {notFoundMessage}", Severity.Error);
                    return default;

                default:
                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: Something went wrong on the server.", Severity.Error);
                    return default;
            }
        }

        public async Task<T?> HandleApiResponseDeleteAsync<T>(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: Update Success.", Severity.Success);
                    return default;

                case System.Net.HttpStatusCode.BadRequest:
                    var badRequestStream = await response.Content.ReadAsStreamAsync();
                    var badRequestMessage = await JsonSerializer.DeserializeAsync<string>(badRequestStream);

                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: {badRequestMessage}", Severity.Warning);
                    return default;

                case System.Net.HttpStatusCode.NotFound:
                    var notFoundStream = await response.Content.ReadAsStreamAsync();
                    var notFoundMessage = await JsonSerializer.DeserializeAsync<string>(notFoundStream);

                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: {notFoundMessage}", Severity.Error);
                    return default;

                default:
                    _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackbar.Add($"{response.StatusCode}: Something went wrong on the server.", Severity.Error);
                    return default;
            }
        }
    }
}
