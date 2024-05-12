using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Queries;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Results;
using ServicesHealthCheck.DataAccess.Abstract;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Handlers.QueryHandlers
{
    public class GetEventViewerLogByFilterQueryHandler : IRequestHandler<GetEventViewerLogByFilterQuery, List<GetEventViewerLogByFilterQueryResult>>
    {
        private readonly IServiceEventViewerLogRepository _serviceEventViewerLogRepository;
        private readonly IMapper _mapper;

        public GetEventViewerLogByFilterQueryHandler(IMapper mapper, IServiceEventViewerLogRepository serviceEventViewerLogRepository)
        {
            _mapper = mapper;
            _serviceEventViewerLogRepository = serviceEventViewerLogRepository;
        }

        public async Task<List<GetEventViewerLogByFilterQueryResult>> Handle(GetEventViewerLogByFilterQuery request, CancellationToken cancellationToken)
        {
            //dinamik filtering mekanizması
            var serviceEventViewerLogs = await _serviceEventViewerLogRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(request.ServiceName))
            {
                serviceEventViewerLogs = serviceEventViewerLogs.Where(x => x.ServiceName == request.ServiceName).ToList();
            }
            if (!string.IsNullOrEmpty(request.EventType))
            {
                serviceEventViewerLogs = serviceEventViewerLogs.Where(x => x.EventType == request.EventType).ToList();
            }
            if (request.EventStartDate != DateTime.MinValue || request.EventEndDate != DateTime.MinValue)
            {
                serviceEventViewerLogs = serviceEventViewerLogs.Where(x =>
                    x.EventDate >= request.EventStartDate && x.EventDate <= request.EventEndDate).ToList();
            }
            serviceEventViewerLogs = serviceEventViewerLogs.Take(10).ToList();
            var result = _mapper.Map<List<GetEventViewerLogByFilterQueryResult>>(serviceEventViewerLogs);
            return result;
        }
    }
}
