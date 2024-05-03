using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.Dtos.ServiceErrorLogDtos;

namespace ServicesHealthCheck.Dtos.ServiceHealthCheckDtos
{
    public class GeneralServiceHealthCheckDto
    {
        public GeneralServiceHealthCheckDto()
        {
            ServiceHealthCheckDtos = new List<ServiceHealthCheckDto>();
            Errors = new List<CreatedServiceErrorLogDto>();
        }
        public List<ServiceHealthCheckDto> ServiceHealthCheckDtos { get; set; }
        public List<CreatedServiceErrorLogDto> Errors { get; set; }
    }
}
