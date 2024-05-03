using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Dtos.ServiceErrorLogDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceErrorLogs.Commands
{
    public class CreatedServiceErrorLogCommand : IRequest<List<UpdatedServiceErrorLogDto>>
    {
        public List<CreatedServiceErrorLogDto> Errors { get; set; }
    }
}
