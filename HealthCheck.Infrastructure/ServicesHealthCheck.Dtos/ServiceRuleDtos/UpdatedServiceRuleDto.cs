using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Dtos.ServiceRuleDtos
{
    public class UpdatedServiceRuleDto
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public string EventMessage { get; set; }
        public string EventType { get; set; }
        public bool IsRestarted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
