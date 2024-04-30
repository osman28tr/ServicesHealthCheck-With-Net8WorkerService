using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Commands;
using ServicesHealthCheck.Shared.Models;

namespace ServicesHealthCheck.WorkerService.BackgroundServices
{
    public class HealthCheckByTimeBackgroundService : BackgroundService
    {
        private readonly IMediator _mediator;
        public HealthCheckByTimeBackgroundService(IMediator mediator)
        {
            _mediator = mediator;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Her saat başında bu döngü içindeki kod çalışır
                Console.WriteLine($"[{DateTime.Now}] Temporal log: This is a sample log message.");

                var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

                var logTime = configuration.GetSection("ServiceHealthCheckLogTimeByMinute").Value;
                var services = configuration.GetSection("Services").Get<List<string>>();

                var serviceCpuUsageLimit = configuration.GetSection("ResourceLimits:MaxCpuUsage").Value;

                var serviceResourceUsageLimit = new ServiceResourceUsageLimit() { CpuMaxUsage = Convert.ToInt16(serviceCpuUsageLimit) };

                _mediator.Send(new CreatedServiceHealthCheckByTimeCommand()
                    { Services = services, ServiceResourceUsageLimit = serviceResourceUsageLimit });

                await Task.Delay(TimeSpan.FromMinutes(Convert.ToDouble(logTime)), stoppingToken);
            }
        }
    }
}
