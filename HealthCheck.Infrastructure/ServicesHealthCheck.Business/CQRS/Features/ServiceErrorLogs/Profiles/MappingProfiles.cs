using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Results;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using ServicesHealthCheck.Dtos.ServiceErrorLogDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreatedServiceErrorLogDto, ServiceErrorLog>().ReverseMap();
            CreateMap<GetListServiceErrorLogQueryResult, ServiceErrorLog>().ReverseMap();
        }
    }
}
