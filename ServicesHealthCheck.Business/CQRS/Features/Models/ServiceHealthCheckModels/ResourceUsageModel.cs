using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.CQRS.Features.Models.ServiceHealthCheckModels
{
    public class ResourceUsageModel
    {
        public string CpuUsage { get; set; }
        public string PhysicalMemoryUsage { get; set; }
        public string VirtualMemoryUsage { get; set; }
        public string PrivateMemoryUsage { get; set; }
    }
}
