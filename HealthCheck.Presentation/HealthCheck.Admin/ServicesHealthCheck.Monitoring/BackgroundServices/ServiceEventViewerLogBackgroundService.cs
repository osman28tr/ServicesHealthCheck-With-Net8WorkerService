using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Commands;

namespace ServicesHealthCheck.Monitoring.BackgroundServices
{
    public class ServiceEventViewerLogBackgroundService : BackgroundService
    {
        private readonly IMediator _mediator;
        public ServiceEventViewerLogBackgroundService(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                var services = configuration.GetSection("Services").Get<List<string>>();
                _mediator.Send(new CreatedServiceEventViewerLogCommand { Services = services });
            };
            Console.ReadLine();
            return null;
        }
    }
}
