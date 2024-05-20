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
    public class ServiceEventViewerLogRepository : IServiceEventViewerLogRepository
    {
        private readonly HealthCheckContext _context;
        public ServiceEventViewerLogRepository(HealthCheckContext context)
        {
            _context = context;
        }
        public async Task<List<ServiceEventViewerLog>> GetAllAsync()
        {
            var result = await _context.ServiceEventViewerLogs.FindAsync(FilterDefinition<ServiceEventViewerLog>.Empty);
            return await result.ToListAsync();
        }

        public async Task<ServiceEventViewerLog> GetAsync(Expression<Func<ServiceEventViewerLog, bool>> filterExpression)
        {
            var result = await _context.ServiceEventViewerLogs.FindAsync(filterExpression);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<ServiceEventViewerLog> GetByIdAsync(string id)
        {
            var filter = Builders<ServiceEventViewerLog>.Filter.Eq(x => x.Id, id);
            var response = await _context.ServiceEventViewerLogs.FindAsync(filter);
            return await response.FirstOrDefaultAsync();
        }

        public async Task<ServiceEventViewerLog> GetByServiceNameAsync(string serviceName)
        {
            var filter = Builders<ServiceEventViewerLog>.Filter.Eq(x => x.ServiceName, serviceName);
            var response = await _context.ServiceEventViewerLogs.FindAsync(filter);
            return await response.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ServiceEventViewerLog>> FindAsync(Expression<Func<ServiceEventViewerLog, bool>> filterExpression)
        {
            var result = await _context.ServiceEventViewerLogs.FindAsync(filterExpression);
            return await result.ToListAsync();
        }

        public async Task AddAsync(ServiceEventViewerLog entity)
        {
            await _context.ServiceEventViewerLogs.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(ServiceEventViewerLog entity)
        {
            var filter = Builders<ServiceEventViewerLog>.Filter.Eq(x => x.Id, entity.Id);
            await _context.ServiceEventViewerLogs.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<ServiceEventViewerLog>.Filter.Eq(x => x.Id, id);
            await _context.ServiceEventViewerLogs.DeleteOneAsync(filter);
        }
    }
}
