using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.DataAccess.Abstract
{
    public interface IServiceEventViewerLogRepository
    {
        Task<List<ServiceEventViewerLog>> GetAllAsync();
        Task<ServiceEventViewerLog> GetByIdAsync(string id);
        Task<ServiceEventViewerLog> GetByServiceNameAsync(string serviceName);
        Task<IEnumerable<ServiceEventViewerLog>> FindAsync(Expression<Func<ServiceEventViewerLog, bool>> filterExpression);
        Task AddAsync(ServiceEventViewerLog entity);
        Task UpdateAsync(ServiceEventViewerLog entity);
        Task DeleteAsync(string id);
    }
}
