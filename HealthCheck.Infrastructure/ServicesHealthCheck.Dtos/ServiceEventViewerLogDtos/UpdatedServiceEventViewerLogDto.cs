using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Dtos.ServiceEventViewerLogDtos
{
    public class UpdatedServiceEventViewerLogDto
    {
        public int EventId { get; set; }
        public string ServiceName { get; set; }
        public string EventType { get; set; }
        public string EventMessage { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EventCurrentDate { get; set; }
    }
}
