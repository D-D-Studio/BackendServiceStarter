using Microsoft.Extensions.DependencyInjection;

namespace BackendServiceStarter.Services.Models
{
    public static class ModelsServiceExtension
    {
        public static IServiceCollection AddModelsServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();

            return services;
        }
    }
}