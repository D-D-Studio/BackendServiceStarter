using System.Linq;
using BackendServiceStarter.Models.Logs;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BackendServiceStarter.Services.Logs
{
    public class MongoLoggerProvider : ILoggerProvider
    {
        private const string LogCollectionName = "_Logs";
        private readonly IMongoDatabase _mongoDatabase;
        
        public MongoLoggerProvider(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public ILogger CreateLogger(string categoryName)
        {
            EnsureCreatedCollection();
            
            return new MongoLogger(_mongoDatabase.GetCollection<Log>(LogCollectionName), categoryName);
        }

        private void EnsureCreatedCollection()
        {
            if (!CheckCollectionExisting())
            {
                _mongoDatabase.CreateCollection(LogCollectionName);
            }
        }

        private bool CheckCollectionExisting()
        {
            var collections = _mongoDatabase.ListCollectionNames().ToList();
            var isExist = collections.Any(collection => collection == LogCollectionName);

            return isExist;
        }
        
        public void Dispose()
        {
        }
    }
}