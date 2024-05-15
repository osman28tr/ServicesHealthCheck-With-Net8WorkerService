using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Dtos.ServiceRuleDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Commands
{
    public class CreatedServiceEventViewerLogCommand : IRequest<List<UpdatedServiceRuleDto>>
    {
        public List<string> Services { get; set; }
    }
}
