using AutoMapper;
using ServicesHealthCheck.Business.CQRS.Features.Commands.ServiceHealthCheckCommands;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.CQRS.Features.Profiles.AutoMapper.ServiceHealthCheckProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreatedServiceHealthCheckCommand, ServiceHealthCheck>().ReverseMap();
            CreateMap<ServiceHealthCheckDto, ServiceHealthCheck>().ReverseMap();
        }
    }
}
