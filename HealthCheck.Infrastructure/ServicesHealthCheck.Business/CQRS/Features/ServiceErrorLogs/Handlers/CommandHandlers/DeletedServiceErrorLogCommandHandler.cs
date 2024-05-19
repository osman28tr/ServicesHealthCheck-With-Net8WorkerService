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
    public class DeletedServiceErrorLogCommandHandler : IRequestHandler<DeletedServiceErrorLogCommand>
    {
        private readonly IServiceErrorLogRepository _serviceErrorLogRepository;
        public DeletedServiceErrorLogCommandHandler(IServiceErrorLogRepository serviceErrorLogRepository)
        {
            _serviceErrorLogRepository = serviceErrorLogRepository;
        }

        public async Task Handle(DeletedServiceErrorLogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var serviceErrorLogs = await _serviceErrorLogRepository.GetAllAsync();
                var completedErrors = serviceErrorLogs.Where(x => x.IsCompleted == true).ToList();
                foreach (var error in completedErrors)
                {
                    await _serviceErrorLogRepository.DeleteAsync(error.Id);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception.StackTrace + " " + exception.Message);
            }
        }
    }
}
