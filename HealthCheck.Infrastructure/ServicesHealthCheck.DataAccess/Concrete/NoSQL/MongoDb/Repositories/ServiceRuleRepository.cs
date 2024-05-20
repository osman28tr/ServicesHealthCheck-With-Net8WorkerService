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
    public class ServiceRuleRepository : IServiceRuleRepository
    {
        private readonly HealthCheckContext _context;
        public ServiceRuleRepository(HealthCheckContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceRule>> GetAllAsync()
        {
            var result = await _context.ServiceRules.FindAsync(FilterDefinition<ServiceRule>.Empty);
            return await result.ToListAsync();
        }

        public async Task<ServiceRule> GetAsync(Expression<Func<ServiceRule, bool>> filterExpression)
        {
            var result = await _context.ServiceRules.FindAsync(filterExpression);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<ServiceRule> GetByIdAsync(string id)
        {
            var filter = Builders<ServiceRule>.Filter.Eq(x => x.Id, id);
            var response = await _context.ServiceRules.FindAsync(filter);
            return await response.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ServiceRule>> FindAsync(Expression<Func<ServiceRule, bool>> filterExpression)
        {
            var result = await _context.ServiceRules.FindAsync(filterExpression);
            return await result.ToListAsync();
        }

        public async Task AddAsync(ServiceRule entity)
        {
            await _context.ServiceRules.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(ServiceRule entity)
        {
            var filter = Builders<ServiceRule>.Filter.Eq(x => x.Id, entity.Id);
            await _context.ServiceRules.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<ServiceRule>.Filter.Eq(x => x.Id, id);
            await _context.ServiceRules.DeleteOneAsync(filter);
        }
    }
}
