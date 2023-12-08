using Blazored.LocalStorage;
using Common.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using Website;
using Website.Authentication;
using Website.Data;
using Website.Helpers;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

var configuration = new ConfigurationBuilder()
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .Build();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddSingleton<JsonSerializerOptionsWrapper>();
builder.Services.AddHttpClient("WebApiClient",
    configureClient =>
{
    configureClient.BaseAddress = new Uri(configuration.GetValue<string>("APIData:Address"));
    configureClient.Timeout = new TimeSpan(0, 0, 30);
});
builder.Services.AddSingleton<ReservationService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CarRegistrationHelper>();
builder.Services.AddScoped<ApiResponseHelper>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();