using AuthenticationAuthorization.DatabaseContext;
using AuthenticationAuthorization.Repositories;
using AuthenticationAuthorization.Services;
using AuthenticationAuthorization.UnitOfWorks;
using AuthenticationAuthorization.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace AuthenticationAuthorization.Extentions
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, string connectionString, Assembly migrationAssembly)
        {
            //add here all service dependencies...
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, (m) => m.MigrationsAssembly(migrationAssembly)));
            
            services.AddScoped<IAuthenticationUnitOfWork, AuthenticationUnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountManagementService, AccountManagementService>();
            services.AddScoped<IUserRepository, UserRipository>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IApplicationTime, ApplicationTime>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();

            return services;
        }

        //This is for JwtAuthenticatio
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme ,options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetValue<string>("JWT:Issuer"),
                    ValidateAudience = true,
                    ValidAudience = configuration.GetValue<string>("JWT:Audience"),
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:Token")!)),
                    ValidateIssuerSigningKey = true,
                };
            });

            return services;
        }
    }
}
