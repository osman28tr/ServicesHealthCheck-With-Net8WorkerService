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
using ServicesHealthCheck.Dtos.SignalRDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreatedServiceHealthCheckCommand, ServiceHealthCheck>().ReverseMap();
            CreateMap<ServiceHealthCheckDto, ServiceHealthCheck>().ReverseMap();
            CreateMap<ServicesHealthCheckSignalRDto, ServiceHealthCheck>().ReverseMap();
            CreateMap<ServicesHealthCheckSignalRDto, ServiceHealthCheckDto>().ReverseMap();
            CreateMap<GetListServiceHealthCheckResult, ServiceHealthCheck>().ReverseMap();
        }
    }
}
