using Common.Helpers;
using DatabaseAccess;
using DatabaseAccess.Interfaces;
using DatabaseAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(path:"appsettings.json", optional:false, reloadOnChange:true)
                .Build();

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true; // returns status code 406 if the consumer request the result in a format thats not json.
            });

            builder.Services.AddDbContext<AutoMechanicManagementSystemDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("AutoMechanicManagementSystemDb"))); // TODO: Get this connectionstring from an azure keyvault.

            builder.Services.AddScoped<PasswordHelper>();
            builder.Services.AddScoped<CarRegistrationHelper>();
            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<IReservationTypeRepository, ReservationTypeRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Authentication:Issuer"],
                        ValidAudience = configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:SecretForKey"]))
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("MustBeBogholderOrAdmin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimTypes.Role, new string[] { "Admin", "Bogholder" });
                });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
