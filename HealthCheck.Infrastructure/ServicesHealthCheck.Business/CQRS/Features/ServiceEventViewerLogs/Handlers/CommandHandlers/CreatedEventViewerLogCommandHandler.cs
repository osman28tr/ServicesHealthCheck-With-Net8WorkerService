using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Models;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Handlers.CommandHandlers
{
    public class CreatedEventViewerLogCommandHandler : IRequestHandler<CreatedServiceEventViewerLogCommand>
    {
        private readonly IServiceEventViewerLogRepository _serviceEventViewerLogRepository;
        private readonly IMapper _mapper;
        public CreatedEventViewerLogCommandHandler(IServiceEventViewerLogRepository serviceEventViewerLogRepository, IMapper mapper)
        {
            _serviceEventViewerLogRepository = serviceEventViewerLogRepository;
            _mapper = mapper;
        }

        public async Task Handle(CreatedServiceEventViewerLogCommand request, CancellationToken cancellationToken)
        {

            foreach (var service in request.Services)
            {
                try
                {
                    EventLog eventLog = new EventLog();
                    eventLog.Source = service;
                    if (EventLog.SourceExists(service))
                    {
                        foreach (EventLogEntry log in eventLog.Entries)
                        {
                            if (log.EntryType == EventLogEntryType.Error || log.EntryType == EventLogEntryType.Warning)
                            {
                                await _serviceEventViewerLogRepository.AddAsync(new ServiceEventViewerLog()
                                {
                                    ServiceName = service,
                                    EventId = log.EventID,
                                    EventType = EventLogEntryType.Error.ToString(),
                                    EventMessage = log.Message,
                                    EventDate = log.TimeGenerated
                                });
                            }
                        }
                    }
                    else
                    {
                        string message = $"{service} isimli servis'e ait event viewer'da log kayıtları bulunamadı.";

                        var serviceNotEventLog = await _serviceEventViewerLogRepository.FindAsync(x => x.ServiceName == service);

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
                catch (Exception exception)
                {
                    Console.WriteLine("an a error occured. " + exception.Message);
                    continue;
                }
            }
        }
    }
}
