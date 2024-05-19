using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Serilog;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Commands;
using ServicesHealthCheck.DataAccess.Abstract;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Handlers.CommandHandlers
{
    public class UpdatedServiceErrorLogCommandHandler : IRequestHandler<UpdatedServiceErrorLogCommand>
    {
        private readonly IServiceErrorLogRepository _serviceErrorLogRepository;
        public UpdatedServiceErrorLogCommandHandler(IServiceErrorLogRepository serviceErrorLogRepository)
        {
            _serviceErrorLogRepository = serviceErrorLogRepository;
        }

        public async Task Handle(UpdatedServiceErrorLogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var serviceErrorLog = await _serviceErrorLogRepository.GetByIdAsync(request.Id);
                if (request.IsCompleted == true)
                    serviceErrorLog.IsCompleted = false;
                else
                    serviceErrorLog.IsCompleted = true;
                
                await _serviceErrorLogRepository.UpdateAsync(serviceErrorLog);
            }
            catch (Exception exception)
            {
                Log.Error(exception.StackTrace + " " + exception.Message);
            }
        }
    }
}
