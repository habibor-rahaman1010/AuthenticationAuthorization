using AuthenticationAuthorization.DatabaseContext;
using AuthenticationAuthorization.Extentions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Reflection;

namespace AuthenticationAuthorization
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                var migrationAssembly = Assembly.GetExecutingAssembly() ?? throw new InvalidOperationException("Migration Assembly not found.");

                // Add services to the container.             
                builder.Services.RegisterServices(connectionString, migrationAssembly);
                builder.Services.AddJwtAuthentication(builder.Configuration);

                builder.Services.AddControllers();

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowReactApp", policy =>
                    {
                        policy.WithOrigins("http://localhost:3000") // Your React dev URL
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); // Optional: if using cookies
                    });
                });

                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                builder.Services.AddOpenApi();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.MapScalarApiReference();
                }

                app.UseCors("AllowReactApp");
                app.UseHttpsRedirection();

                app.UseAuthentication();

                app.UseAuthorization();

                app.MapControllers();

                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), "The application occures a problem!");
            }
            finally
            {
                Console.WriteLine("The application has crashed!");
            }
        }
    }
}
