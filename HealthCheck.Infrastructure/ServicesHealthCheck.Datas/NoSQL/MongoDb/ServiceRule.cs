﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using ServicesHealthCheck.Shared.Entities;

namespace ServicesHealthCheck.Datas.NoSQL.MongoDb
{
    public class ServiceRule : IEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public string EventMessage { get; set; }
        public string EventType { get; set; }
        public bool IsRestarted { get; set; }
        public DateTime CreatedDate { get; set; }
        public RestartTime RestartTime { get; set; }
    }
    public class RestartTime
    {
        public int Week { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
    }
}
