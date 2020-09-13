using BackendServiceStarter.Services.Workers.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace BackendServiceStarter.Services.Workers
{
    public static class JobsServiceExtension
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
            services.AddSingleton<ClearLogsJob>();
            services.AddSingleton(new JobSchedule
            (
                jobType: typeof(ClearLogsJob),
                crontabExpression: "*/5 * * * * ?"
            ));
        }
    }
}