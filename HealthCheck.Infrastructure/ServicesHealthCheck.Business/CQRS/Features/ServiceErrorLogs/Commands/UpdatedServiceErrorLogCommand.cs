using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Commands
{
    public class UpdatedServiceErrorLogCommand : IRequest
    {
        public string Id { get; set; }
        public bool IsCompleted { get; set; }
    }
}
