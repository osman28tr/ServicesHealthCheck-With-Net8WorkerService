using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Results;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceRules.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ServiceRule, GetListServiceRuleQueryResult>().ReverseMap();

            CreateMap<ServiceRule, CreatedServiceRuleCommand>().ReverseMap();
        }
    }
}
