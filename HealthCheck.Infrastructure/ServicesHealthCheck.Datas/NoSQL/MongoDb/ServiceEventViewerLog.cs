using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using ServicesHealthCheck.Shared.Entities;

namespace ServicesHealthCheck.Datas.NoSQL.MongoDb
{
    public class ServiceEventViewerLog : IEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public int EventId { get; set; }
        public string ServiceName { get; set; }
        public string EventType { get; set; }
        public string EventMessage { get; set; }
        public DateTime EventDate { get; set; }
    }
}
