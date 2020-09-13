using System;
using System.Threading.Tasks;
using BackendServiceStarter.Models.Logs;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BackendServiceStarter.Services.Logs
{
    public class MongoLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly IMongoCollection<Log> _logs;
        
        public MongoLogger(IMongoCollection<Log> logs, string categoryName)
        {
            _logs = logs;
            _categoryName = categoryName;
        }
        
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var log = new Log
            {
                Level = logLevel,
                Category = _categoryName,
                Message = formatter != null ? formatter(state, exception) : exception.Message,
                DateTime = DateTime.Now
            };
            
           _logs.InsertOne(log);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}