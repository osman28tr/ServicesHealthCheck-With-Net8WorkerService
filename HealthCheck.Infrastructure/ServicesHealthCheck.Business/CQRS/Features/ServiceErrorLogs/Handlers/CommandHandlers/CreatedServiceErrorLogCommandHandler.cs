using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Commands;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Handlers.CommandHandlers
{
    public class CreatedServiceErrorLogCommandHandler : IRequestHandler<CreatedServiceErrorLogCommand>
    {
        private readonly IServiceErrorLogRepository _serviceErrorLogRepository;
        private readonly IMapper _mapper;
        public CreatedServiceErrorLogCommandHandler(IServiceErrorLogRepository serviceErrorLogRepository, IMapper mapper)
        {
            _serviceErrorLogRepository = serviceErrorLogRepository;
            _mapper = mapper;
        }

        public async Task Handle(CreatedServiceErrorLogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var errors = await _serviceErrorLogRepository.GetAllAsync();
                var errorMapping = _mapper.Map<List<ServiceErrorLog>>(request.Errors);
                errorMapping.ForEach(async x =>
                {
                    if (!errors.Any(error => error.ServiceName == x.ServiceName && error.ErrorMessage == x.ErrorMessage))
                    {
                        await _serviceErrorLogRepository.AddAsync(x);
                    }
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine("An error occured: Error status related to services could not be added." +
                                  exception.Message);
            }
        }
    }
}
