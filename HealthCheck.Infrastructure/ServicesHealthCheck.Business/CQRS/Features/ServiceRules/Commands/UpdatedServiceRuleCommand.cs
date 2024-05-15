using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Dtos.ServiceRuleDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Commands
{
    public class UpdatedServiceRuleCommand : IRequest
    {
        public List<UpdatedServiceRuleDto> UpdateServiceRuleDtos { get; set; }
    }
}
