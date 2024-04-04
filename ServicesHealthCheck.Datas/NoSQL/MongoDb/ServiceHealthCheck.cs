﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using ServicesHealthCheck.Shared.Entities;

namespace ServicesHealthCheck.Datas.NoSQL.MongoDb
{
    public class ServiceHealthCheck : IEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string ServiceName { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Status { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string CpuUsage { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string PhysicalMemoryUsage { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string VirtualMemoryUsage { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string PrivateMemoryUsage { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Boolean)]
        public bool IsHealthy { get; set; }
    }
}