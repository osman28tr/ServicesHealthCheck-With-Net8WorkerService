using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Results
{
    public class GetEventViewerLogByFilterQueryResult
    {
        public string Id { get; set; }
        public int EventId { get; set; }
        public string ServiceName { get; set; }
        public string EventType { get; set; }
        public string EventMessage { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EventCurrentDate { get; set; }
    }
}
