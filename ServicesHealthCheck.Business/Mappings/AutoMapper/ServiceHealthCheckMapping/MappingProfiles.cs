using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ServicesHealthCheck.Business.CQRS.Features.Commands.ServiceHealthCheckCommands;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;

namespace ServicesHealthCheck.Business.Mappings.AutoMapper.ServiceHealthCheckMapping
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
