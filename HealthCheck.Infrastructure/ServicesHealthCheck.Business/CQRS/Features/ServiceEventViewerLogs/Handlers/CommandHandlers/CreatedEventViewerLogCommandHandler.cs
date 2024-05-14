using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MongoDB.Driver.Linq;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Models;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using ServicesHealthCheck.Dtos.ServiceRuleDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Handlers.CommandHandlers
{
    public class CreatedEventViewerLogCommandHandler : IRequestHandler<CreatedServiceEventViewerLogCommand, List<UpdatedServiceRuleDto>>
    {
        private readonly IServiceEventViewerLogRepository _serviceEventViewerLogRepository;
        private readonly IServiceRuleRepository _serviceRuleRepository;
        private readonly IMapper _mapper;
        public CreatedEventViewerLogCommandHandler(IServiceEventViewerLogRepository serviceEventViewerLogRepository, IMapper mapper, IServiceRuleRepository serviceRuleRepository)
        {
            _serviceEventViewerLogRepository = serviceEventViewerLogRepository;
            _mapper = mapper;
            _serviceRuleRepository = serviceRuleRepository;
        }

        public async Task<List<UpdatedServiceRuleDto>> Handle(CreatedServiceEventViewerLogCommand request, CancellationToken cancellationToken)
        {
            List<UpdatedServiceRuleDto> updatedServiceRules = new List<UpdatedServiceRuleDto>();
            var rules = await _serviceRuleRepository.GetAllAsync();
            RestartServiceByRule(rules);
            foreach (var service in request.Services)
            {
                try
                {
                    EventLog eventLog = new EventLog();
                    eventLog.Log = "Application";
                    if (EventLog.SourceExists(service))
                    {
                        foreach (EventLogEntry log in eventLog.Entries)
                        {
                            if (log.Source.ToLower() == service.ToLower())
                            {
                                if (log.EntryType == EventLogEntryType.Error || log.EntryType == EventLogEntryType.Warning)
                                {
                                    foreach (var rule in rules)
                                    {
                                        if (rule.ServiceName == service && rule.EventType == log.EntryType.ToString() && log.Message.Contains(rule.EventMessage))
                                        {
                                            if (rule.IsRestarted == false)
                                            {
                                                ServiceController serviceController = new ServiceController(service);
                                                if (serviceController.Status == ServiceControllerStatus.Running)
                                                {
                                                    serviceController.Stop();
                                                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                                                }
                                                serviceController.Start();
                                                serviceController.WaitForStatus(ServiceControllerStatus.Running);
                                                rule.IsRestarted = true;
                                                var mappedServiceRuleDto = _mapper.Map<UpdatedServiceRuleDto>(rule);
                                                mappedServiceRuleDto.IsRestarted = true;
                                                updatedServiceRules.Add(mappedServiceRuleDto);
                                            }
                                        }
                                    }
                                    var serviceEventLog = await _serviceEventViewerLogRepository.FindAsync(x => x.ServiceName == service);
                                    if (serviceEventLog == null)
                                    {
                                        await _serviceEventViewerLogRepository.AddAsync(new ServiceEventViewerLog()
                                        {
                                            ServiceName = service,
                                            EventId = log.EventID,
                                            EventType = log.EntryType.ToString(),
                                            EventMessage = log.Message,
                                            EventDate = log.TimeGenerated
                                        });
                                    }
                                    else
                                    {
                                        if (!(serviceEventLog.Select(x => x.EventMessage).Contains(log.Message) && serviceEventLog.Select(y => y.ServiceName).Contains(service) && serviceEventLog.Select(z => z.EventId).Contains(log.EventID)))
                                        {
                                            await _serviceEventViewerLogRepository.AddAsync(new ServiceEventViewerLog()
                                            {
                                                ServiceName = service,
                                                EventId = log.EventID,
                                                EventType = log.EntryType.ToString(),
                                                EventMessage = log.Message,
                                                EventDate = log.TimeGenerated
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string message = $"{service} isimli servis'e ait event viewer'da log kayıtları bulunamadı.";

                        var serviceNotEventLog = await _serviceEventViewerLogRepository.FindAsync(x => x.ServiceName == service);

                        if (serviceNotEventLog == null)
                        {
                            await _serviceEventViewerLogRepository.AddAsync(new ServiceEventViewerLog()
                            {
                                ServiceName = service,
                                EventId = 0,
                                EventType = EventLogEntryType.Warning.ToString(),
                                EventMessage = message,
                                EventDate = DateTime.Now.AddHours(3)
                            });
                        }
                        else
                        {
                            if (!serviceNotEventLog.Select(x => x.EventMessage).Contains(message))
                            {
                                Console.WriteLine(message);
                                await _serviceEventViewerLogRepository.AddAsync(new ServiceEventViewerLog()
                                {
                                    ServiceName = service,
                                    EventId = 0,
                                    EventType = EventLogEntryType.Warning.ToString(),
                                    EventMessage = message,
                                    EventDate = DateTime.Now.AddHours(3)
                                });
                                serviceNotEventLog = await _serviceEventViewerLogRepository.FindAsync(x => x.ServiceName == service);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("an error occured. " + exception.Message);
                }
            }
            return updatedServiceRules;
        }
    public void RestartServiceByRule(List<ServiceRule> rules)
    {
        try
        {
            rules.ForEach(x =>
            {
                ServiceController serviceController = new ServiceController(x.ServiceName);
                var lastDate = DateTime.Now - x.CreatedDate;
                if (x.RestartTime != null)
                {
                    if (x.RestartTime.Day != 0)
                    {
                        if (lastDate.Days == x.RestartTime.Day)
                        {
                            if (serviceController.Status == ServiceControllerStatus.Running)
                            {
                                serviceController.Stop();
                                serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                            }
                            serviceController.Start();
                            serviceController.WaitForStatus(ServiceControllerStatus.Running);
                        }
                    }
                    else if (x.RestartTime.Week != 0)
                    {
                        if (lastDate.Days == x.RestartTime.Week * 7)
                        {
                            if (serviceController.Status == ServiceControllerStatus.Running)
                            {
                                serviceController.Stop();
                                serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                            }
                            serviceController.Start();
                            serviceController.WaitForStatus(ServiceControllerStatus.Running);
                        }
                    }
                    else
                    {
                        if (lastDate.Days == x.RestartTime.Month * 30)
                        {
                            if (serviceController.Status == ServiceControllerStatus.Running)
                            {
                                serviceController.Stop();
                                serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                            }
                            serviceController.Start();
                            serviceController.WaitForStatus(ServiceControllerStatus.Running);
                        }
                    }
                }

            });
        }
        catch (Exception exception)
        {
            Console.WriteLine("an error occured. " + exception.Message);
        }
    }
}
}
