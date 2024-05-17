using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Serilog;
using ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Commands;
using ServicesHealthCheck.DataAccess.Abstract;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Handlers.CommandHandlers
{
    public class UpdatedServiceRuleCommandHandler : IRequestHandler<UpdatedServiceRuleCommand>
    {
        private readonly IServiceRuleRepository _serviceRuleRepository;
        private readonly IMapper _mapper;
        public UpdatedServiceRuleCommandHandler(IMapper mapper, IServiceRuleRepository serviceRuleRepository)
        {
            _mapper = mapper;
            _serviceRuleRepository = serviceRuleRepository;
        }

        public async Task Handle(UpdatedServiceRuleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.UpdateServiceRuleDtos != null)
                {
                    foreach (var item in request.UpdateServiceRuleDtos)
                    {
                        var serviceRule = _mapper.Map<ServiceRule>(item);
                        await _serviceRuleRepository.UpdateAsync(serviceRule);
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error("An error occurred while updating the rule for eventviewer logs. " + exception.Message);
            }
        }
    }
}
