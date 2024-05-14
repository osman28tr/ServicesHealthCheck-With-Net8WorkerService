using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Commands;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Handlers.CommandHandlers
{
    public class CreatedServiceRuleCommandHandler : IRequestHandler<CreatedServiceRuleCommand>
    {
        private readonly IServiceRuleRepository _serviceRuleRepository;
        private readonly IMapper _mapper;
        public CreatedServiceRuleCommandHandler(IMapper mapper, IServiceRuleRepository serviceRuleRepository)
        {
            _mapper = mapper;
            _serviceRuleRepository = serviceRuleRepository;
        }

        public async Task Handle(CreatedServiceRuleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var serviceRule = _mapper.Map<ServiceRule>(request);
                serviceRule.CreatedDate = DateTime.Now.AddHours(3);
                await _serviceRuleRepository.AddAsync(serviceRule);
            }
            catch (Exception exception)
            {
                Console.WriteLine("an error occured. " + exception.Message);
            }
        }
    }
}
