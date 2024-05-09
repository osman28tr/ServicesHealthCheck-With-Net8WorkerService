using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Results;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Queries
{
    public class GetListHealthCheckByFilterQuery : IRequest<List<GetListHealthCheckByFilterQueryResult>>
    {
        public string? ServiceName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
