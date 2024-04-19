using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServicesHealthCheck.Shared.Settings;
using ServicesHealthCheck.Shared.Settings.Abstract;

namespace ServicesHealthCheck.Shared
{
    public static class SharedServiceRegistration
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            services.AddSingleton<IMailSetting>(sp =>
            {
                return sp.GetRequiredService<IOptions<MailSetting>>().Value;
            });
            return services;
        }
    }
}
