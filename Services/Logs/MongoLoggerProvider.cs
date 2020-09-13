using BackendServiceStarter.Models.Logs;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BackendServiceStarter.Services.Logs
{
    public class MongoLoggerProvider : ILoggerProvider
    {
        private readonly IMongoDatabase _mongoDatabase;
        
        public MongoLoggerProvider(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public ILogger CreateLogger(string categoryName)
        {
            // TODO: Check collection existing
            
            return new MongoLogger(_mongoDatabase.GetCollection<Log>("Logs"), categoryName);
        }
        
        public void Dispose()
        {
        }
    }
}