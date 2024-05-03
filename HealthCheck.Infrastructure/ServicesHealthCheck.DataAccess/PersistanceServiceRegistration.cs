using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.DataAccess.Concrete.NoSQL.MongoDb.Repositories;

namespace ServicesHealthCheck.DataAccess
{
    public static class PersistanceServiceRegistration
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services)
        {
            services.AddSingleton<IServiceHealthCheckRepository, ServiceHealthCheckRepository>();
            services.AddSingleton<IServiceHealthCheckByTimeRepository, ServiceHealthCheckByTimeRepository>();
            services.AddSingleton<IServiceErrorLogRepository, ServiceErrorLogRepository>();
            return services;
        }
    }
}
