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
        public string CpuUsage { get; set; }
        public string PhysicalMemoryUsage { get; set; }
        public string VirtualMemoryUsage { get; set; }
        public string PrivateMemoryUsage { get; set; }
        public bool IsHealthy { get; set; }
    }
}
