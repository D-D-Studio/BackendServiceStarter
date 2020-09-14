using BackendServiceStarter.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BackendServiceStarter.Configurations
{
    public static class DatabasesExtension
    {
        public static IServiceCollection AddDatabasesConnections(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration.GetValue<string>("MongoOptions:Host"));
            var mongoDatabase = mongoClient.GetDatabase(configuration.GetValue<string>("MongoOptions:Database"));

            services.AddSingleton(mongoDatabase);
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("ApplicationConnection"));
            });
            
            return services;
        }
    }
}