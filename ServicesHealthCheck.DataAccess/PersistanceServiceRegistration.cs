using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.DataAccess.Concrete.NoSQL.MongoDb.Repositories;

namespace ServicesHealthCheck.DataAccess
{
    public static class PersistanceServiceRegistration
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services)
        {
            services.AddSingleton<IServiceHealthCheckRepository, ServiceHealthCheckRepository>();
            return services;
        }
    }
}
