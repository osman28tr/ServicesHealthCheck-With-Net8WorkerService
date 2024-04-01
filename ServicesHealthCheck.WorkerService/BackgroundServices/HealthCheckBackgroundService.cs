using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using ServicesHealthCheck.Business.CQRS.Features.Commands.ServiceHealthCheckCommands;
using ServicesHealthCheck.Business.RealTimes.SignalR;

namespace ServicesHealthCheck.WorkerService.BackgroundServices
{
    public class HealthCheckBackgroundService : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<HealthCheckHub> _healthCheckHub;
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

                // await _healthCheck.CheckServicesHealth(services);
                var updateServiceHealthCheck = await _mediator.Send(new CreatedServiceHealthCheckCommand() { Services = services });
                if (updateServiceHealthCheck.Any())
                {
                    _mediator.Send(new UpdatedServiceHealthCheckCommand()
                        { ServiceHealthCheckDtos = updateServiceHealthCheck });
                }
                System.Threading.Thread.Sleep(1000);
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
