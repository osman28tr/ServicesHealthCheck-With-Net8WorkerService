using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Results;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Queries
{
    public class GetEventViewerLogByFilterQuery : IRequest<List<GetEventViewerLogByFilterQueryResult>>
    {
        public string ServiceName { get; set; }
        public string EventType { get; set; }
        public DateTime EventStartDate{ get; set; }
        public DateTime EventEndDate { get; set; }
    }
}
