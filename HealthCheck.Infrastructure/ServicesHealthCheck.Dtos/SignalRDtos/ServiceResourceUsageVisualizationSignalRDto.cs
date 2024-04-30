using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Dtos.SignalRDtos
{
    public class ServiceResourceUsageVisualizationSignalRDto
    {
        public string ServiceName { get; set; }
        public float CpuUsage { get; set; }
        public float PhysicalMemoryUsage { get; set; }
        public float VirtualMemoryUsage { get; set; }
        public float PrivateMemoryUsage { get; set; }
    }
}
