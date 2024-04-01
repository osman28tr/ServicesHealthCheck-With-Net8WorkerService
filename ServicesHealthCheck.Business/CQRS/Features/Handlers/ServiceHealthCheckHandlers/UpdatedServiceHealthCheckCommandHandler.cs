using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.Commands.ServiceHealthCheckCommands;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.Business.CQRS.Features.Handlers.ServiceHealthCheckHandlers
{
    public class UpdatedServiceHealthCheckCommandHandler : IRequestHandler<UpdatedServiceHealthCheckCommand>
    {
        private readonly IServiceHealthCheckRepository _serviceHealthCheckRepository;
        private readonly IMapper _mapper;
        public UpdatedServiceHealthCheckCommandHandler(IServiceHealthCheckRepository serviceHealthCheckRepository, IMapper mapper)
        {
            _serviceHealthCheckRepository = serviceHealthCheckRepository;
            _mapper = mapper;
        }
        public async Task Handle(UpdatedServiceHealthCheckCommand request, CancellationToken cancellationToken)
        {
            foreach (var serviceHealthCheckDto in request.ServiceHealthCheckDtos)
            {
                var serviceHealthCheck = _mapper.Map<ServiceHealthCheck>(serviceHealthCheckDto);

                var findHealthCheck =
                    await _serviceHealthCheckRepository.GetByServiceNameAsync(serviceHealthCheck.ServiceName);

                serviceHealthCheck.Id = findHealthCheck.Id;

                await _serviceHealthCheckRepository.UpdateAsync(serviceHealthCheck);
            }
        }
    }
}
