using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Queries;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Results;
using ServicesHealthCheck.DataAccess.Abstract;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Handlers.QueryHandlers
{
    public class GetListServiceErrorLogQueryHandler : IRequestHandler<GetListServiceErrorLogQuery, List<GetListServiceErrorLogQueryResult>>
    {
        private readonly IServiceErrorLogRepository _serviceErrorLogRepository;
        private readonly IMapper _mapper;
        public GetListServiceErrorLogQueryHandler(IMapper mapper, IServiceErrorLogRepository serviceErrorLogRepository)
        {
            _mapper = mapper;
            _serviceErrorLogRepository = serviceErrorLogRepository;
        }

        public async Task<List<GetListServiceErrorLogQueryResult>> Handle(GetListServiceErrorLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var errors = await _serviceErrorLogRepository.GetAllAsync();
                return _mapper.Map<List<GetListServiceErrorLogQueryResult>>(errors);
            }
            catch (Exception exception)
            {
                Console.WriteLine("An error occured:" + exception.Message);
                return null;
            }
        }
    }
}
