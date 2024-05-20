using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Models;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Results;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;
using ServicesHealthCheck.Dtos.ServiceEventViewerLogDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ServiceEventViewerLog, EventViewerLogModel>().ReverseMap();
            CreateMap<ServiceEventViewerLog, GetEventViewerLogByFilterQueryResult>().ReverseMap();

            CreateMap<UpdatedServiceEventViewerLogDto, ServiceEventViewerLog>().ReverseMap();
        }
    }
}
