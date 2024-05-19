using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Queries;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Results;
using ServicesHealthCheck.Business.RealTimes.SignalR.Abstract;
using ServicesHealthCheck.DataAccess.Abstract;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Handlers.QueryHandlers
{
    public class GetListServiceHealthCheckQueryHandler : IRequestHandler<GetListServiceHealthCheckQuery,List<GetListServiceHealthCheckResult>>
    {
        private readonly IServiceHealthCheckRepository _serviceHealthCheckRepository;
        private readonly IMapper _mapper;
        private readonly ISignalRService _signalRService;
        public GetListServiceHealthCheckQueryHandler(IServiceHealthCheckRepository serviceHealthCheckRepository, IMapper mapper, ISignalRService signalRService)
        {
            _serviceHealthCheckRepository = serviceHealthCheckRepository;
            _mapper = mapper;
            _signalRService = signalRService;
        }
        public async Task<List<GetListServiceHealthCheckResult>> Handle(GetListServiceHealthCheckQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var serviceHealthChecks = await _serviceHealthCheckRepository.GetAllAsync();
                return _mapper.Map<List<GetListServiceHealthCheckResult>>(serviceHealthChecks);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace + " " + exception.Message);
                return null;
            }
        }
    }
}
