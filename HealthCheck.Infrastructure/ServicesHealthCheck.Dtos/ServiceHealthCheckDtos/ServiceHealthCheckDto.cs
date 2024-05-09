using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.Dtos.ServiceErrorLogDtos;

namespace ServicesHealthCheck.Dtos.ServiceHealthCheckDtos
{
    public class ServiceHealthCheckDto
    {
        public string ServiceName { get; set; }
        public string Status { get; set; }
        public float CpuUsage { get; set; }
        public float PhysicalMemoryUsage { get; set; }
        public float VirtualMemoryUsage { get; set; }
        public float PrivateMemoryUsage { get; set; }
        public bool IsHealthy { get; set; }
        public bool IsResourceUsageLimitExceeded { get; set; }
    }
}
