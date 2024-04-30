using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Shared.Models;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Commands
{
    public class CreatedServiceHealthCheckByTimeCommand : IRequest
    {
        public List<string> Services { get; set; }
        public ServiceResourceUsageLimit ServiceResourceUsageLimit { get; set; }
    }
}
