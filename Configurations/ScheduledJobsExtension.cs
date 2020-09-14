using BackendServiceStarter.Services.Workers;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace BackendServiceStarter.Configurations
{
    public static class ScheduledJobsExtension
    {
        public static IServiceCollection AddScheduledJobs(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            AddJobs(services);

            services.AddHostedService<JobsHostedService>();

            return services;
        }

        private static void AddJobs(IServiceCollection services)
        {
            // Example of worker:
            // 
            // services.AddSingleton<ClearLogsJob>();
            // services.AddSingleton(new JobSchedule
            // (
            //     jobType: typeof(ClearLogsJob),
            //     crontabExpression: "*/5 * * * * ?"
            // ));
        }
    }
}