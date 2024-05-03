using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Commands;
using ServicesHealthCheck.Shared.Models;

namespace ServicesHealthCheck.Monitoring.BackgroundServices
{
    public class HealthCheckBackgroundService : BackgroundService
    {
        private readonly IMediator _mediator;
        public HealthCheckBackgroundService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("HealthCheck Service Started.");

            return base.StartAsync(cancellationToken);
        }

        protected override async Task<List<Dictionary<string, string>>> ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("HealthCheck Service executing...");
            while (!stoppingToken.IsCancellationRequested)
            {
                var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                var services = configuration.GetSection("Services").Get<List<string>>();
                var serviceCpuUsageLimit = configuration.GetSection("ResourceLimits:MaxCpuUsage").Value;

                var serviceResourceUsageLimit = new ServiceResourceUsageLimit() { CpuMaxUsage = Convert.ToInt16(serviceCpuUsageLimit) };

                // await _healthCheck.CheckServicesHealth(services);
                var generalServiceHealthCheckDto = await _mediator.Send(new CreatedServiceHealthCheckCommand()
                { Services = services, ServiceResourceUsageLimit = serviceResourceUsageLimit });

                if (generalServiceHealthCheckDto.ServiceHealthCheckDtos.Any() || generalServiceHealthCheckDto.Errors.Any())
                {
                    if (generalServiceHealthCheckDto.ServiceHealthCheckDtos.Any())
                    {
                        await _mediator.Send(new UpdatedServiceHealthCheckCommand()
                        { ServiceHealthCheckDtos = generalServiceHealthCheckDto.ServiceHealthCheckDtos });
                    }
                    if(generalServiceHealthCheckDto.Errors.Any())
                    {
                        await _mediator.Send(new CreatedServiceErrorLogCommand()
                        { Errors = generalServiceHealthCheckDto.Errors });
                    }
                }
            }
            Console.ReadLine();
            return null;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("HealthCheck Service Stopped.");
            return base.StopAsync(cancellationToken);
        }
    }
}
