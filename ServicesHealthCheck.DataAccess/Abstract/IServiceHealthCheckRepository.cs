using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.DataAccess.Abstract
{
    public interface IServiceHealthCheckRepository
    {
        Task<List<ServiceHealthCheck>> GetAllAsync();
        Task<ServiceHealthCheck> GetByIdAsync(string id);
        Task<ServiceHealthCheck> GetByServiceNameAsync(string id);
        Task<IEnumerable<ServiceHealthCheck>> FindAsync(Expression<Func<ServiceHealthCheck,bool>> filterExpression);
        Task AddAsync(ServiceHealthCheck entity);
        Task UpdateAsync(ServiceHealthCheck entity);
        Task DeleteAsync(string id);
    }
}
