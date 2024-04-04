using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Results;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Queries
{
    public class GetListServiceHealthCheckQuery : IRequest<List<GetListServiceHealthCheckResult>>
    {
    }
}
