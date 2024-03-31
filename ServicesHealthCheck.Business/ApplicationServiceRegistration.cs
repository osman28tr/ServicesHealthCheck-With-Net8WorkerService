using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.Business.HealthChecks.Abstract;
using ServicesHealthCheck.Business.HealthChecks;
using ServicesHealthCheck.Business.Notifications.EMailService;
using ServicesHealthCheck.Business.Notifications.EMailService.Abstract;

namespace ServicesHealthCheck.Business
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IHealthCheck,HealthCheck>();
            services.AddTransient<IMailService, MailService>();
            return services;
        }
    }
}
