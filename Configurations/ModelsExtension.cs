using BackendServiceStarter.Services.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BackendServiceStarter.Configurations
{
    public static class ModelsExtension
    {
        public static IServiceCollection AddModelsServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();

            return services;
        }
    }
}