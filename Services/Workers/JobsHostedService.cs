using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

namespace BackendServiceStarter.Services.Workers
{
    public class JobsHostedService : IHostedService
    {
        private IScheduler _scheduler;
        
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobSchedule> _jobSchedules;

        public JobsHostedService(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<JobSchedule> jobSchedules)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _jobSchedules = jobSchedules;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            _scheduler.JobFactory = _jobFactory;

            foreach (var jobSchedule in _jobSchedules)
            {
                var jobDetail = JobBuilder
                    .Create(jobSchedule.JobType)
                    .WithIdentity(jobSchedule.JobType.FullName!)
                    .WithDescription(jobSchedule.JobType.Name)
                    .Build();

                var trigger = TriggerBuilder
                    .Create()
                    .WithIdentity($"{jobSchedule.JobType.FullName}.trigger")
                    .WithDescription(jobSchedule.CrontabExpression)
                    .WithCronSchedule(jobSchedule.CrontabExpression)
                    .Build();

                await _scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
            }

            await _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Shutdown(cancellationToken);
        }
    }
}