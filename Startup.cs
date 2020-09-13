using BackendServiceStarter.Databases;
using BackendServiceStarter.Models.Options;
using BackendServiceStarter.Services.Auth;
using BackendServiceStarter.Services.Crypto;
using BackendServiceStarter.Services.Logs;
using BackendServiceStarter.Services.Models;
using BackendServiceStarter.Services.Workers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
            ConfigureDatabases(services);
            ConfigureAuthServices(services);
            ConfigureModelsServices(services);
            
            services.AddScheduledJobs();
            
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.UseMongoLogger(app.ApplicationServices.GetRequiredService<IMongoDatabase>());
            
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

        private void ConfigureDatabases(IServiceCollection services)
        {
            var mongoClient = new MongoClient(_configuration.GetValue<string>("MongoOptions:Host"));
            var mongoDatabase = mongoClient.GetDatabase(_configuration.GetValue<string>("MongoOptions:Database"));

            services.AddSingleton(mongoDatabase);
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("ApplicationConnection"));
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
            services.AddScoped<UserService>();
        }
    }
}