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
    public class ServiceHealthCheckRepository : IServiceHealthCheckRepository
    {
        private readonly HealthCheckContext _context;

        public ServiceHealthCheckRepository(HealthCheckContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceHealthCheck>> GetAllAsync()
        {
            var result = await _context.ServiceHealthCheck.FindAsync(FilterDefinition<ServiceHealthCheck>.Empty);
            return await result.ToListAsync();
        }

        public async Task<ServiceHealthCheck> GetByIdAsync(string id)
        {
            var filter = Builders<ServiceHealthCheck>.Filter.Eq(x => x.Id, id);
            var response = _context.ServiceHealthCheck.FindAsync(filter);
            return await response.Result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ServiceHealthCheck>> FindAsync(Expression<Func<ServiceHealthCheck,bool>> filterExpression)
        {
            var result = await _context.ServiceHealthCheck.FindAsync(filterExpression);
            return await result.ToListAsync();
        }

        public async Task AddAsync(ServiceHealthCheck entity)
        {
            await _context.ServiceHealthCheck.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(ServiceHealthCheck entity)
        {
            var filter = Builders<ServiceHealthCheck>.Filter.Eq(x => x.Id, entity.Id);
            await _context.ServiceHealthCheck.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<ServiceHealthCheck>.Filter.Eq(x => x.Id, id);
            await _context.ServiceHealthCheck.DeleteOneAsync(filter);
        }
    }
}
