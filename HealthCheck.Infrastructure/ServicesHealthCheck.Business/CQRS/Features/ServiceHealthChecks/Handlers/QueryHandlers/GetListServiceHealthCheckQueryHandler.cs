using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Queries;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Results;
using ServicesHealthCheck.DataAccess.Abstract;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Handlers.QueryHandlers
{
    public class GetListServiceHealthCheckQueryHandler : IRequestHandler<GetListServiceHealthCheckQuery,List<GetListServiceHealthCheckResult>>
    {
        private readonly IServiceHealthCheckRepository _serviceHealthCheckRepository;
        private readonly IMapper _mapper;
        public GetListServiceHealthCheckQueryHandler(IServiceHealthCheckRepository serviceHealthCheckRepository, IMapper mapper)
        {
            _serviceHealthCheckRepository = serviceHealthCheckRepository;
            _mapper = mapper;
        }
        public async Task<List<GetListServiceHealthCheckResult>> Handle(GetListServiceHealthCheckQuery request, CancellationToken cancellationToken)
        {
            var serviceHealthChecks = await _serviceHealthCheckRepository.GetAllAsync();
            return _mapper.Map<List<GetListServiceHealthCheckResult>>(serviceHealthChecks);
        }
    }
}
