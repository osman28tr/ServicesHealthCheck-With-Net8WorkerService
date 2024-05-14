using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Results;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Queries
{
    public class GetListServiceRuleQuery : IRequest<List<GetListServiceRuleQueryResult>>
    {
    }
}
