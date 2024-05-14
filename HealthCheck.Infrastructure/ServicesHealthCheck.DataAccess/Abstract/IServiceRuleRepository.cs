using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.DataAccess.Abstract
{
    public interface IServiceRuleRepository : IRepository<ServiceRule>
    {
    }
}
