using BackendServiceStarter.Databases;
using BackendServiceStarter.Models.Options;
using BackendServiceStarter.Services.Auth;
using BackendServiceStarter.Services.Crypto;
using BackendServiceStarter.Services.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace BackendServiceStarter
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("ApplicationConnection"));
            });
            
            ConfigureAuthServices(services);
            ConfigureModelsServices(services);

            services.AddControllers().AddNewtonsoftJson();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureAuthServices(IServiceCollection services)
        {
            var jwtAuthOptions = new JwtAuthOptions()
            {
                Issuer = _configuration.GetValue<string>("JwtAuthOptions:Issuer"),
                Audience = _configuration.GetValue<string>("JwtAuthOptions:Audience"),
                Key = _configuration.GetValue<string>("JwtAuthOptions:Key"),
                Lifetime = _configuration.GetValue<uint>("JwtAuthOptions:Lifetime")
            };
            
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
            services.AddScoped<IAuthService, AuthService>();
        }

        private void ConfigureModelsServices(IServiceCollection services)
        {
            services.AddScoped(typeof(ModelService<>));
        }
    }
}