using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Commands
{
    public class CreatedServiceHealthCheckCommand : IRequest<List<ServiceHealthCheckDto>>
    {
        public List<string> Services { get; set; }
    }
}
