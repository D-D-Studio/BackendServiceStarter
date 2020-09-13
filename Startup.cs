using BackendServiceStarter.Configurations;
using BackendServiceStarter.Databases;
using BackendServiceStarter.Services.Logs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

            services.AddJwtAuth(options =>
            {
                options.Issuer = _configuration.GetValue<string>("JwtAuthOptions:Issuer");
                options.Audience = _configuration.GetValue<string>("JwtAuthOptions:Audience");
                options.Key = _configuration.GetValue<string>("JwtAuthOptions:Key");
                options.Lifetime = _configuration.GetValue<uint>("JwtAuthOptions:Lifetime");
            });
            
            services.AddModelsServices();
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
    }
}