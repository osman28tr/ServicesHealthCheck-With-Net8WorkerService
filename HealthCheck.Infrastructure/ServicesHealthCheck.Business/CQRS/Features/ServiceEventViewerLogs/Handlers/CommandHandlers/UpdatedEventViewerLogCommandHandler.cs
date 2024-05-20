using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Serilog;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Commands;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Handlers.CommandHandlers
{
    public class UpdatedEventViewerLogCommandHandler : IRequestHandler<UpdatedEventViewerLogCommand>
    {
        private readonly IServiceEventViewerLogRepository _serviceEventViewerLogRepository;
        private readonly IMapper _mapper;
        public UpdatedEventViewerLogCommandHandler(IServiceEventViewerLogRepository serviceEventViewerLogRepository, IMapper mapper)
        {
            _serviceEventViewerLogRepository = serviceEventViewerLogRepository;
            _mapper = mapper;
        }
        public async Task Handle(UpdatedEventViewerLogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.UpdatedServiceEventViewerLogDtos.Count != 0)
                {
                    foreach (var eventViewer in request.UpdatedServiceEventViewerLogDtos)
                    {
                        var eventViewerLog = await _serviceEventViewerLogRepository.GetAsync(x =>
                            x.ServiceName == eventViewer.ServiceName && x.EventId == eventViewer.EventId &&
                            x.EventType == eventViewer.EventType && x.EventMessage == eventViewer.EventMessage);
                        if (eventViewerLog != null)
                        {
                            var updatedEventViewerLog = _mapper.Map<ServiceEventViewerLog>(eventViewer);
                            updatedEventViewerLog.EventCurrentDate = eventViewer.EventCurrentDate;
                            updatedEventViewerLog.Id = eventViewerLog.Id;
                            await _serviceEventViewerLogRepository.UpdateAsync(updatedEventViewerLog);
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
}
