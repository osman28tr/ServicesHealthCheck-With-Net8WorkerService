﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceEventViewerLogs.Models
{
    public class EventViewerLogModel
    {
        public string ServiceName { get; set; }
        public string EventType { get; set; }
        public string EventMessage { get; set; }
        public DateTime EventDate { get; set; }
    }
}
