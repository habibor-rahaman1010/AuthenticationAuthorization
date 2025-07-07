using AuthenticationAuthorization.Extentions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Reflection;
using Serilog.Events;

namespace AuthenticationAuthorization
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."); ;
            var migrationAssembly = Assembly.GetExecutingAssembly() ?? throw new InvalidOperationException("Migration Assembly not found.");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug().WriteTo.MSSqlServer(
                    connectionString: connection,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "ApplicationLogs", AutoCreateSqlTable = true })
                .ReadFrom.Configuration(configuration)
                .CreateBootstrapLogger();

            try
            {
                Log.Information("Application Starting...");
                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) =>
                {
                    loggerConfiguration.MinimumLevel.Debug()
                    .WriteTo.MSSqlServer(
                        connectionString: connection,
                        sinkOptions: new MSSqlServerSinkOptions { TableName = "ApplicationLogs", AutoCreateSqlTable = true })
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(builder.Configuration);

                });

                // Add services to the container.             
                builder.Services.RegisterServices(connection, migrationAssembly);
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

                builder.Services.AddControllers(options =>
                {
                    options.SuppressAsyncSuffixInActionNames = false;
                });


                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                builder.Services.AddOpenApi();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.MapScalarApiReference(options =>
                    {
                        // Fluent API
                        options
                            .WithTitle("Jwt Authentication")
                            .WithSidebar(true);

                        // Object initializer
                        options.Title = "Jwt Authentication";
                        options.ShowSidebar = true;
                    });
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
                Log.Fatal(ex, "Application Crashed...");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
    }
}
