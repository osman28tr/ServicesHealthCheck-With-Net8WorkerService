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
    public class ServiceErrorLogRepository : IServiceErrorLogRepository
    {
        private readonly HealthCheckContext _context;

        public ServiceErrorLogRepository(HealthCheckContext context)
        {
            _context = context;
        }
        public async Task<List<ServiceErrorLog>> GetAllAsync()
        {
            var result = await _context.ServiceErrorLogs.FindAsync(FilterDefinition<ServiceErrorLog>.Empty);
            return await result.ToListAsync();
        }

        public async Task<ServiceErrorLog> GetAsync(Expression<Func<ServiceErrorLog, bool>> filterExpression)
        {
            var result = await _context.ServiceErrorLogs.FindAsync(filterExpression);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<ServiceErrorLog> GetByIdAsync(string id)
        {
            var filter = Builders<ServiceErrorLog>.Filter.Eq(x => x.Id, id);
            var response = await _context.ServiceErrorLogs.FindAsync(filter);
            return await response.FirstOrDefaultAsync();
        }

        public async Task<ServiceErrorLog> GetByServiceNameAsync(string serviceName)
        {
            var filter = Builders<ServiceErrorLog>.Filter.Eq(x => x.ServiceName, serviceName);
            var response = await _context.ServiceErrorLogs.FindAsync(filter);
            return await response.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ServiceErrorLog>> FindAsync(Expression<Func<ServiceErrorLog, bool>> filterExpression)
        {
            var result = await _context.ServiceErrorLogs.FindAsync(filterExpression);
            return await result.ToListAsync();
        }

        public async Task AddAsync(ServiceErrorLog entity)
        {
            await _context.ServiceErrorLogs.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(ServiceErrorLog entity)
        {
            var filter = Builders<ServiceErrorLog>.Filter.Eq(x => x.Id, entity.Id);
            await _context.ServiceErrorLogs.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<ServiceErrorLog>.Filter.Eq(x => x.Id, id);
            await _context.ServiceErrorLogs.DeleteOneAsync(filter);
        }
    }
}
