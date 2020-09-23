using System;

namespace BackendServiceStarter.Services.Workers
{
    public class JobSchedule
    {
        public Type JobType { get; set; }
        
        public string CrontabExpression { get; set; }
        
        public JobSchedule(Type jobType, string crontabExpression)
        {
            JobType = jobType;
            CrontabExpression = crontabExpression;
        }
    }
}