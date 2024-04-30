using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.DataAccess.Abstract
{
    public interface IServiceHealthCheckByTimeRepository
    {
        Task<List<ServiceHealthCheckByTime>> GetAllAsync();
        Task<ServiceHealthCheckByTime> GetByIdAsync(string id);
        Task<ServiceHealthCheckByTime> GetByServiceNameAsync(string id);
        Task<IEnumerable<ServiceHealthCheckByTime>> FindAsync(Expression<Func<ServiceHealthCheckByTime, bool>> filterExpression);
        Task AddAsync(ServiceHealthCheckByTime entity);
        Task UpdateAsync(ServiceHealthCheckByTime entity);
        Task DeleteAsync(string id);
    }
}
