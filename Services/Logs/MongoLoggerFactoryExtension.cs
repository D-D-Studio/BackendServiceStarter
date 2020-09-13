using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BackendServiceStarter.Services.Logs
{
    public static class MongoLoggerFactoryExtension
    {
        public static ILoggerFactory UseMongoLogger(this ILoggerFactory factory, IMongoDatabase mongoDatabase)
        {
            factory.AddProvider(new MongoLoggerProvider(mongoDatabase));
            
            return factory;
        }
    }
}