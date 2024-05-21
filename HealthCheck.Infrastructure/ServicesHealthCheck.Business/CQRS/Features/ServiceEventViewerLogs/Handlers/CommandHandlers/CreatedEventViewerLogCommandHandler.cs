using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Amazon.Auth.AccessControlPolicy;
using AutoMapper;
using MediatR;
using MongoDB.Driver.Linq;
using Serilog;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Models;
using ServicesHealthCheck.Business.EventViewerCustomViews.Abstract;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using ServicesHealthCheck.Dtos.ServiceEventViewerLogDtos;
using ServicesHealthCheck.Dtos.ServiceRuleDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Handlers.CommandHandlers
{
    public class CreatedEventViewerLogCommandHandler : IRequestHandler<CreatedServiceEventViewerLogCommand, GeneralCreatedEventViewerLogDto>
    {
        private readonly IServiceEventViewerLogRepository _serviceEventViewerLogRepository;
        private readonly IServiceRuleRepository _serviceRuleRepository;
        private readonly IEvCustomView _evCustomView;
        private readonly IMapper _mapper;
        public CreatedEventViewerLogCommandHandler(IServiceEventViewerLogRepository serviceEventViewerLogRepository, IMapper mapper, IServiceRuleRepository serviceRuleRepository, IEvCustomView customView)
        {
            _serviceEventViewerLogRepository = serviceEventViewerLogRepository;
            _mapper = mapper;
            _serviceRuleRepository = serviceRuleRepository;
            _evCustomView = customView;
        }

        public async Task<GeneralCreatedEventViewerLogDto> Handle(CreatedServiceEventViewerLogCommand request, CancellationToken cancellationToken)
        {
            GeneralCreatedEventViewerLogDto generalCreatedEventViewerLogDto = new GeneralCreatedEventViewerLogDto();
            //List<UpdatedServiceRuleDto> updatedServiceRules = new List<UpdatedServiceRuleDto>();
            //List<UpdatedServiceEventViewerLogDto> updatedServiceEventViewerLogDtos = new List<UpdatedServiceEventViewerLogDto>();


            var rules = await _serviceRuleRepository.GetAllAsync();
            if (rules != null)
                RestartServiceByRule(rules);

            if (request.Services != null)
            {
                await _evCustomView.CreateCustomViewAsync(request.Services); // custom view oluşması
                foreach (var service in request.Services)
                {
                    string xml = CreateDynamicXml(service);
                    var query = new EventLogQuery("Application", PathType.LogName, xml);
                    EventLogReader logReader = new EventLogReader(query);
                    try
                    {
                        if (EventLog.SourceExists(service))
                        {
                            for (EventRecord eventDetail = logReader.ReadEvent(); eventDetail != null; eventDetail = logReader.ReadEvent())
                            {
                                if (eventDetail.LevelDisplayName == "Hata" || eventDetail.LevelDisplayName == "Bilgi")
                                {
                                    if (rules != null)
                                    {
                                        foreach (var rule in rules)
                                        {
                                            if (rule.ServiceName == service && rule.EventType == eventDetail.LevelDisplayName && eventDetail.FormatDescription().Contains(rule.EventMessage))
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
                                                    generalCreatedEventViewerLogDto.UpdatedServiceRules.Add(mappedServiceRuleDto);
                                                }
                                            }
                                        }
                                    }
                                    var serviceEventLog = await _serviceEventViewerLogRepository.FindAsync(x => x.ServiceName == service);
                                    if (serviceEventLog.Count() == 0)
                                    {
                                        await _serviceEventViewerLogRepository.AddAsync(new ServiceEventViewerLog()
                                        {
                                            ServiceName = service,
                                            EventId = eventDetail.Id,
                                            EventType = eventDetail.LevelDisplayName,
                                            EventMessage = eventDetail.FormatDescription(),
                                            EventDate = eventDetail.TimeCreated,
                                            EventCurrentDate = eventDetail.TimeCreated
                                        });
                                    }
                                    else
                                    {
                                        if (!(serviceEventLog.Select(x => x.EventMessage).Contains(eventDetail.FormatDescription()) && serviceEventLog.Select(y => y.ServiceName).Contains(service) && serviceEventLog.Select(z => z.EventId).Contains(eventDetail.Id)))
                                        {
                                            await _serviceEventViewerLogRepository.AddAsync(new ServiceEventViewerLog()
                                            {
                                                ServiceName = service,
                                                EventId = eventDetail.Id,
                                                EventType = eventDetail.LevelDisplayName,
                                                EventMessage = eventDetail.FormatDescription(),
                                                EventDate = eventDetail.TimeCreated,
                                                EventCurrentDate = eventDetail.TimeCreated
                                            });
                                        }
                                        else
                                        {
                                            generalCreatedEventViewerLogDto.UpdatedServiceEventViewerLogDtos.Add(new UpdatedServiceEventViewerLogDto()
                                            {
                                                ServiceName = service,
                                                EventId = eventDetail.Id,
                                                EventType = eventDetail.LevelDisplayName,
                                                EventMessage = eventDetail.FormatDescription(),
                                                EventDate = Convert.ToDateTime(serviceEventLog.First().EventDate),
                                                EventCurrentDate = eventDetail.TimeCreated
                                            });
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
                                    EventDate = DateTime.UtcNow.ToLocalTime(),
                                    EventCurrentDate = DateTime.UtcNow.ToLocalTime()
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
                                        EventDate = DateTime.UtcNow.ToLocalTime(),
                                        EventCurrentDate = DateTime.UtcNow.ToLocalTime()
                                    });
                                    serviceNotEventLog = await _serviceEventViewerLogRepository.FindAsync(x => x.ServiceName == service);
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception.StackTrace + " " + exception.Message);
                    }
                }
            }
            return generalCreatedEventViewerLogDto;
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
        static string CreateDynamicXml(string provider)
        {
            string xmlTemplate = @"
                <QueryList>
                    <Query Id='0' Path='Application'>
                        <Select Path='Application'>
                            *[System[Provider[@Name='{0}'] 
                            and (Level=3 or Level=4) 
                            and TimeCreated[timediff(@SystemTime) &lt;= 604800000]]]
                        </Select>
                    </Query>
                </QueryList>";

            return string.Format(xmlTemplate, provider);
        }
    }
}
