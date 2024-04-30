﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthChecks.Models
{
    public class ResourceUsageModel
    {
        public string CpuUsage { get; set; }
        public string PhysicalMemoryUsage { get; set; }
        public string VirtualMemoryUsage { get; set; }
        public string PrivateMemoryUsage { get; set; }
        public string DiskUsage { get; set; }
        public string AverageDiskQueueUsage { get; set; }
        public bool IsHealthy { get; set; }
    }
}
