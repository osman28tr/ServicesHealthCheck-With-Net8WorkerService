using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Results
{
    public class GetListServiceHealthCheckResult
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public string Status { get; set; }
        public float CpuUsage { get; set; }
        public float PhysicalMemoryUsage { get; set; }
        public float VirtualMemoryUsage { get; set; }
        public float PrivateMemoryUsage { get; set; }
        public bool IsHealthy { get; set; }
    }
}
