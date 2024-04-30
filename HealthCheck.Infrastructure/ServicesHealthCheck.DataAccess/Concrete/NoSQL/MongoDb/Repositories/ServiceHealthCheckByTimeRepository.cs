using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.DataAccess.Concrete.NoSQL.MongoDb.Contexts;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.DataAccess.Concrete.NoSQL.MongoDb.Repositories
{
    public class ServiceHealthCheckByTimeRepository : IServiceHealthCheckByTimeRepository
    {
        private readonly HealthCheckContext _context;
        public ServiceHealthCheckByTimeRepository(HealthCheckContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceHealthCheckByTime>> GetAllAsync()
        {
            var result = await _context.ServiceHealthCheckByTime.FindAsync(MongoDB.Driver.FilterDefinition<ServiceHealthCheckByTime>.Empty);
            return await result.ToListAsync();
        }

        public async Task<ServiceHealthCheckByTime> GetByIdAsync(string id)
        {
            var filter = MongoDB.Driver.Builders<ServiceHealthCheckByTime>.Filter.Eq(x => x.Id, id);
            var response = await _context.ServiceHealthCheckByTime.FindAsync(filter);
            return await response.FirstOrDefaultAsync();
        }

        public async Task<ServiceHealthCheckByTime> GetByServiceNameAsync(string serviceName)
        {
            var filter = MongoDB.Driver.Builders<ServiceHealthCheckByTime>.Filter.Eq(x => x.ServiceName, serviceName);
            var response = await _context.ServiceHealthCheckByTime.FindAsync(filter);
            return await response.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ServiceHealthCheckByTime>> FindAsync(Expression<Func<ServiceHealthCheckByTime, bool>> filterExpression)
        {
            var result = await _context.ServiceHealthCheckByTime.FindAsync(filterExpression);
            return await result.ToListAsync();
        }

        public async Task AddAsync(ServiceHealthCheckByTime entity)
        {
            await _context.ServiceHealthCheckByTime.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(ServiceHealthCheckByTime entity)
        {
            var filter = Builders<ServiceHealthCheckByTime>.Filter.Eq(x => x.Id, entity.Id);
            await _context.ServiceHealthCheckByTime.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<ServiceHealthCheckByTime>.Filter.Eq(x => x.Id, id);
            await _context.ServiceHealthCheckByTime.DeleteOneAsync(filter);
        }
    }
}
