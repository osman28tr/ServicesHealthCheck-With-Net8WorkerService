using AutoMapper;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Commands;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.Dtos.ServiceErrorLogDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Handlers.CommandHandlers
{
    public class UpdatedServiceHealthCheckCommandHandler : IRequestHandler<UpdatedServiceHealthCheckCommand,List< CreatedServiceErrorLogDto>>
    {
        private readonly IServiceHealthCheckRepository _serviceHealthCheckRepository;
        private readonly IMapper _mapper;
        public UpdatedServiceHealthCheckCommandHandler(IServiceHealthCheckRepository serviceHealthCheckRepository, IMapper mapper)
        {
            _serviceHealthCheckRepository = serviceHealthCheckRepository;
            _mapper = mapper;
        }
        public async Task<List<CreatedServiceErrorLogDto>> Handle(UpdatedServiceHealthCheckCommand request, CancellationToken cancellationToken)
        {
            var errorLogs = new List<CreatedServiceErrorLogDto>();
            var serviceName = "";
            try
            {
                foreach (var serviceHealthCheckDto in request.ServiceHealthCheckDtos)
                {
                    serviceName = serviceHealthCheckDto.ServiceName;

                    var serviceHealthCheck = _mapper.Map<ServiceHealthCheck>(serviceHealthCheckDto);

                    var findHealthCheck =
                        await _serviceHealthCheckRepository.GetByServiceNameAsync(serviceHealthCheck.ServiceName);

                    serviceHealthCheck.Id = findHealthCheck.Id;

                    await _serviceHealthCheckRepository.UpdateAsync(serviceHealthCheck);
                }
            }
            catch (Exception exception)
            {
                errorLogs.Add(new CreatedServiceErrorLogDto()
                    {ServiceName = serviceName, ErrorMessage = exception.Message, IsCompleted = false, ErrorDate = DateTime.Now.AddHours(3) });
            }
            return errorLogs;
        }
    }
}
