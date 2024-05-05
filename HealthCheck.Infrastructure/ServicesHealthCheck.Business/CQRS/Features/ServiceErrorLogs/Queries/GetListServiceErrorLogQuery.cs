using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Results;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Queries
{
    public class GetListServiceErrorLogQuery : IRequest<GeneralGetListServiceErrorLogQueryResult>
    {
    }
}
