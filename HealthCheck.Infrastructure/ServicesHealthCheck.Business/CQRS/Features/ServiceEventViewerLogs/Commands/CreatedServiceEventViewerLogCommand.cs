using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Commands
{
    public class CreatedServiceEventViewerLogCommand : IRequest
    {
        public List<string> Services { get; set; }
    }
}
