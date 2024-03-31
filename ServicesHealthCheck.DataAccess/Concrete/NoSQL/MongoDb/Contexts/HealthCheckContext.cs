using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.DataAccess.Concrete.NoSQL.MongoDb.Contexts
{
    public class HealthCheckContext
    {
        private readonly IMongoDatabase _database;
        public HealthCheckContext()
        {
            var connectionString = "mongodb://localhost:27017"; // MongoDB bağlantı adresi
            var databaseName = "HealthCheckDb"; // MongoDB veritabanı adı
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<ServiceHealthCheck> ServiceHealthCheck => _database.GetCollection<ServiceHealthCheck>("ServiceHealthChecks");
    }
}
