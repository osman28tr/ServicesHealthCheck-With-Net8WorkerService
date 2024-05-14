using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Commands
{
    public class CreatedServiceRuleCommand : IRequest
    {
        public string ServiceName { get; set; }
        public string EventType { get; set; }
        public string EventMessage { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
