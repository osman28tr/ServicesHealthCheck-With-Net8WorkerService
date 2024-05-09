using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Models;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Results;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ResourceUsageModelByTime, ServiceHealthCheckByTime>().ReverseMap();
            CreateMap<GetListHealthCheckByFilterQueryResult, ServiceHealthCheckByTime>().ReverseMap();
        }
    }
}
