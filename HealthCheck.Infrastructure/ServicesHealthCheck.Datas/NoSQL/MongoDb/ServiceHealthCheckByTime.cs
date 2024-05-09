using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.Shared.Entities;

namespace ServicesHealthCheck.Datas.NoSQL.MongoDb
{
    public class ServiceHealthCheckByTime : ServiceHealthCheck, IEntity
    {
        public DateTime Date { get; set; }
    }
}
