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
using ServicesHealthCheck.Dtos.ServiceErrorLogDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Handlers.QueryHandlers
{
    public class GetListServiceErrorLogQueryHandler : IRequestHandler<GetListServiceErrorLogQuery, GeneralGetListServiceErrorLogQueryResult>
    {
        private readonly IServiceErrorLogRepository _serviceErrorLogRepository;
        private readonly IMapper _mapper;
        public GetListServiceErrorLogQueryHandler(IMapper mapper, IServiceErrorLogRepository serviceErrorLogRepository)
        {
            _mapper = mapper;
            _serviceErrorLogRepository = serviceErrorLogRepository;
        }

        public async Task<GeneralGetListServiceErrorLogQueryResult> Handle(GetListServiceErrorLogQuery request, CancellationToken cancellationToken)
        {
            var generallistErrors = new GeneralGetListServiceErrorLogQueryResult();
            try
            {
                var errors = await _serviceErrorLogRepository.GetAllAsync();
                generallistErrors.ErrorList = _mapper.Map<List<GetListServiceErrorLogQueryResult>>(errors);
                return generallistErrors;
            }
            catch (Exception exception)
            {
                generallistErrors.Errors.Add(new CreatedServiceErrorLogDto()
                {
                    ServiceName = "All services", ErrorMessage = exception.Message, IsCompleted = false,
                    ErrorDate = DateTime.UtcNow.ToLocalTime()
                });
            }
            return generallistErrors;
        }
    }
}
