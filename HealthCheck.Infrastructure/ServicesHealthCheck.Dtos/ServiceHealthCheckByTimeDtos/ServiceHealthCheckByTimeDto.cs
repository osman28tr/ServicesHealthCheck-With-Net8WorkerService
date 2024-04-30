using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;

namespace ServicesHealthCheck.Dtos.ServiceHealthCheckByTimeDtos
{
    public class ServiceHealthCheckByTimeDto : ServiceHealthCheckDto
    {
        public string Date { get; set; }
    }
}
