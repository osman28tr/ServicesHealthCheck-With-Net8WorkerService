using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Commands
{
    public class CreatedServiceRuleCommand : IRequest
    {
        public CreatedServiceRuleCommand()
        {
            RestartTime = new RestartTime();
        }
        public string ServiceName { get; set; }
        public string EventType { get; set; }
        public string EventMessage { get; set; }
        public DateTime CreatedDate { get; set; }
        public RestartTime RestartTime { get; set; }
    }
}
