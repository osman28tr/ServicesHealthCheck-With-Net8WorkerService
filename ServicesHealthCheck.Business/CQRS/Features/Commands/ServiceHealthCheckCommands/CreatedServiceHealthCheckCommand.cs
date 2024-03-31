using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ServicesHealthCheck.Business.CQRS.Features.Commands.ServiceHealthCheckCommands
{
    public class CreatedServiceHealthCheckCommand : IRequest
    {
        public List<string> Services { get; set; }
    }
}
