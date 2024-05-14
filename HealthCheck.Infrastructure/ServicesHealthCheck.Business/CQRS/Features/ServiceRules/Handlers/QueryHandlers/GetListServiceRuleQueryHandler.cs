using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Queries;
using ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Results;
using ServicesHealthCheck.DataAccess.Abstract;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Handlers.QueryHandlers
{
    public class GetListServiceRuleQueryHandler : IRequestHandler<GetListServiceRuleQuery,List<GetListServiceRuleQueryResult>>
    {
        private readonly IServiceRuleRepository _serviceRuleRepository;
        private readonly IMapper _mapper;
        public GetListServiceRuleQueryHandler(IMapper mapper, IServiceRuleRepository serviceRuleRepository)
        {
            _mapper = mapper;
            _serviceRuleRepository = serviceRuleRepository;
        }

        public async Task<List<GetListServiceRuleQueryResult>> Handle(GetListServiceRuleQuery request, CancellationToken cancellationToken)
        {
            var serviceRules = await _serviceRuleRepository.GetAllAsync();
            var result = _mapper.Map<List<GetListServiceRuleQueryResult>>(serviceRules);
            return result;
        }
    }
}
