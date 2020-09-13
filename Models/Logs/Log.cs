using System;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackendServiceStarter.Models.Logs
{
    public class Log
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonRepresentation(BsonType.String)]
        public LogLevel Level { get; set; }
        
        public string Category { get; set; }
        
        public string Message { get; set; }
        
        public DateTime DateTime { get; set; }
    }
}