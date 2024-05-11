using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Models;
using ServicesHealthCheck.Datas.NoSQL.MongoDb;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ServiceEventViewerLog, EventViewerLogModel>().ReverseMap();
        }
    }
}
