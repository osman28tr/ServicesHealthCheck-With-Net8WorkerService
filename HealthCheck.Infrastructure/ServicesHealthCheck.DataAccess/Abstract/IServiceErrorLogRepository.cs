using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.DataAccess.Abstract
{
    public interface IServiceErrorLogRepository
    {
        Task<List<ServiceErrorLog>> GetAllAsync();
        Task<ServiceErrorLog> GetByIdAsync(string id);
        Task<ServiceErrorLog> GetByServiceNameAsync(string id);
        Task<IEnumerable<ServiceErrorLog>> FindAsync(Expression<Func<ServiceErrorLog, bool>> filterExpression);
        Task AddAsync(ServiceErrorLog entity);
        Task UpdateAsync(ServiceErrorLog entity);
        Task DeleteAsync(string id);
    }
}
