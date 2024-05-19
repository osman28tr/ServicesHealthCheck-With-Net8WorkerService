using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Serilog;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Commands;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using ServicesHealthCheck.Dtos.ServiceErrorLogDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Handlers.CommandHandlers
{
    public class CreatedServiceErrorLogCommandHandler : IRequestHandler<CreatedServiceErrorLogCommand, List<UpdatedServiceErrorLogDto>>
    {
        private readonly IServiceErrorLogRepository _serviceErrorLogRepository;
        private readonly IMapper _mapper;
        public CreatedServiceErrorLogCommandHandler(IServiceErrorLogRepository serviceErrorLogRepository, IMapper mapper)
        {
            _serviceErrorLogRepository = serviceErrorLogRepository;
            _mapper = mapper;
        }

        public async Task<List<UpdatedServiceErrorLogDto>> Handle(CreatedServiceErrorLogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var errors = await _serviceErrorLogRepository.GetAllAsync();
                var errorMapping = _mapper.Map<List<ServiceErrorLog>>(request.Errors);
                var updatedErrorLogs = new List<UpdatedServiceErrorLogDto>();
                errorMapping.ForEach(async x =>
                {
                    if (!errors.Any(error => error.ServiceName == x.ServiceName && error.ErrorMessage == x.ErrorMessage))
                    {
                        await _serviceErrorLogRepository.AddAsync(x);
                    }
                    else
                    {
                        var errorLog = errors.FirstOrDefault(error => error.ServiceName == x.ServiceName && error.ErrorMessage == x.ErrorMessage);
                        if (errorLog.IsCompleted == true)
                        {
                            updatedErrorLogs.Add(new UpdatedServiceErrorLogDto()
                            { Id = errorLog.Id, IsCompleted = errorLog.IsCompleted });
                        }
                    }
                });
                return updatedErrorLogs;
            }
            catch (Exception exception)
            {
                Log.Error(exception.StackTrace + " " + exception.Message);
                return null;
            }
        }
    }
}
