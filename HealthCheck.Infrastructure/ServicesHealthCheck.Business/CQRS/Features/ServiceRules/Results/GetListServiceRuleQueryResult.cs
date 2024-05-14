using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Results
{
    public class GetListServiceRuleQueryResult
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public string EventType { get; set; }
        public string EventMessage { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
