using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Commands;
using ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Results;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreatedServiceHealthCheckCommand, ServiceHealthCheck>().ReverseMap();
            CreateMap<ServiceHealthCheckDto, ServiceHealthCheck>().ReverseMap();
            CreateMap<GetListServiceHealthCheckResult, ServiceHealthCheck>().ReverseMap();
        }
    }
}
