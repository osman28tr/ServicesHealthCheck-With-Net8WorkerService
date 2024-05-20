using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Serilog;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Commands;
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
            try
            {
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
                        if (generalServiceHealthCheckDto.ServiceHealthCheckDtos.Any()) // güncellenecek servis varsa güncelle
                        {
                            var updatedServiceHealthCheckErrors = await _mediator.Send(new UpdatedServiceHealthCheckCommand()
                            { ServiceHealthCheckDtos = generalServiceHealthCheckDto.ServiceHealthCheckDtos }); // servisleri güncelle, hata varsa al
                            if (updatedServiceHealthCheckErrors.Any()) // servisleri güncelleme sırasında hata varsa logla
                            {
                                var updatedServiceErrorLogsByUpdateHealthCheck = await _mediator.Send(new CreatedServiceErrorLogCommand()
                                { Errors = updatedServiceHealthCheckErrors });
                                if (updatedServiceErrorLogsByUpdateHealthCheck.Any()) // loglanan hatalarda aynıları varsa güncelle
                                {
                                    updatedServiceErrorLogsByUpdateHealthCheck.ForEach(async x =>
                                    {
                                        await _mediator.Send(new UpdatedServiceErrorLogCommand()
                                        { Id = x.Id, IsCompleted = x.IsCompleted });
                                    });
                                }
                            }
                        }
                        if (generalServiceHealthCheckDto.Errors.Any())
                        {
                            var updatedServiceErrorLogs = await _mediator.Send(new CreatedServiceErrorLogCommand()
                            { Errors = generalServiceHealthCheckDto.Errors });
                            if (updatedServiceErrorLogs.Any())
                            {
                                updatedServiceErrorLogs.ForEach(async x =>
                                {
                                    await _mediator.Send(new UpdatedServiceErrorLogCommand()
                                    { Id = x.Id, IsCompleted = x.IsCompleted });
                                });
                            }
                        }
                    }

                    //EventViewer Logs
                    Task.Run(async () =>
                    {
                        var eventviewerLogRunState = configuration.GetSection("EventViewerLogRunState").Value;
                        if (Convert.ToBoolean(eventviewerLogRunState) == true)
                        {
                            var generalCreatedEventViewerLog = await _mediator.Send(new CreatedServiceEventViewerLogCommand
                            { Services = services });
                            if (generalCreatedEventViewerLog.UpdatedServiceRules.Count != 0)
                            {
                                await _mediator.Send(new UpdatedServiceRuleCommand
                                { UpdateServiceRuleDtos = generalCreatedEventViewerLog.UpdatedServiceRules });
                            }

                            if (generalCreatedEventViewerLog.UpdatedServiceEventViewerLogDtos.Count != 0)
                            {
                                await _mediator.Send(new UpdatedEventViewerLogCommand
                                { UpdatedServiceEventViewerLogDtos = generalCreatedEventViewerLog.UpdatedServiceEventViewerLogDtos });
                            }
                        }
                    });
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception, "An error occured in HealthCheckBackgroundService.");
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
