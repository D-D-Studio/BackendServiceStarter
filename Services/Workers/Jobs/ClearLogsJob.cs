using System;
using System.Threading.Tasks;
using Quartz;

namespace BackendServiceStarter.Services.Workers.Jobs
{
    public class ClearLogsJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Clear log.");
            
            return Task.CompletedTask;
        }
    }
}