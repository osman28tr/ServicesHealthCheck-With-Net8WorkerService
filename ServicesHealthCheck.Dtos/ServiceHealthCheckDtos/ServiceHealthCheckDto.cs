using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Dtos.ServiceHealthCheckDtos
{
    public class ServiceHealthCheckDto
    {
        public string ServiceName { get; set; }
        public string Status { get; set; }
    }
}
