using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ServicesHealthCheck.Business.HealthChecks.Abstract;
using ServicesHealthCheck.Business.RealTimes.SignalR;

namespace ServicesHealthCheck.WorkerService.BackgroundServices
{
    public class HealthCheckBackgroundService : BackgroundService
    {
        private readonly IHealthCheck _healthCheck;
        private readonly IHubContext<HealthCheckHub> _healthCheckHub;
        public HealthCheckBackgroundService(IHealthCheck healthCheck)
        {
            _healthCheck = healthCheck;
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

                var states = await _healthCheck.CheckServicesHealth(services);
                foreach (var state in states)
                {
                    foreach (var item in state)
                    {
                        await _healthCheckHub.Clients.All.SendAsync("ReceiveMessage", item.Key, item.Value);
                    }
                }
                return states;
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
