using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Dtos.ServiceErrorLogDtos;
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Commands
{
    public class UpdatedServiceHealthCheckCommand : IRequest<List<CreatedServiceErrorLogDto>>
    {
        public List<ServiceHealthCheckDto> ServiceHealthCheckDtos { get; set; }
    }
}
