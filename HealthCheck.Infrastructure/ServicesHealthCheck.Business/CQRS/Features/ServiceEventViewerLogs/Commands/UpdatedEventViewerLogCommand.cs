using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Dtos.ServiceEventViewerLogDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Commands
{
    public class UpdatedEventViewerLogCommand : IRequest
    {
        public List<UpdatedServiceEventViewerLogDto> UpdatedServiceEventViewerLogDtos { get; set; }
    }
}
