using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.DataAccess.Abstract
{
    public interface IServiceErrorLogRepository : IRepository<ServiceErrorLog>
    {
        Task<ServiceErrorLog> GetByServiceNameAsync(string id);
    }
}
