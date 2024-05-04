using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Datas.NoSQL.MongoDb
{
    public class ServiceHealthCheck
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string ServiceName { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Status { get; set; }
        public float CpuUsage { get; set; }
        public float PhysicalMemoryUsage { get; set; }
        public float VirtualMemoryUsage { get; set; }
        public float PrivateMemoryUsage { get; set; }
        public float DiskUsage { get; set; }
        public bool IsHealthy { get; set; }
        public bool IsResourceUsageLimitExceeded { get; set; }
    }
}
