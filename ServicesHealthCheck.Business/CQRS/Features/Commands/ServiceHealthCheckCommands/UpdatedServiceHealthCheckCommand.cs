using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.Commands.ServiceHealthCheckCommands
{
    public class UpdatedServiceHealthCheckCommand : IRequest
    {
        public List<ServiceHealthCheckDto> ServiceHealthCheckDtos { get; set; }
    }
}
