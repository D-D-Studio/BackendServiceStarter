using System;
using BackendServiceStarter.Models.Options;
using BackendServiceStarter.Services.Auth;
using BackendServiceStarter.Services.Crypto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BackendServiceStarter.Configurations
{
    public static class JwtAuthExtension
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services, Action<JwtAuthOptions> optionsAction)
        {
            var jwtAuthOptions = new JwtAuthOptions();
            
            optionsAction(jwtAuthOptions);
            
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = jwtAuthOptions.Issuer,
                        ValidAudience = jwtAuthOptions.Audience,
                        IssuerSigningKey = jwtAuthOptions.GetSymmetricSecurityKey(),

                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true
                    };
                });

            services.AddSingleton(jwtAuthOptions);
            services.AddScoped<IHashService, BCryptHashService>();
            services.AddScoped<IAuthService, JwtAuthService>();
            
            return services;
        }
    }
}