﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ServicesHealthCheck.Dtos.ServiceEventViewerLogDtos;
using ServicesHealthCheck.Dtos.ServiceRuleDtos;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Commands
{
    public class CreatedServiceEventViewerLogCommand : IRequest<GeneralCreatedEventViewerLogDto>
    {
        public List<string> Services { get; set; }
    }
}
