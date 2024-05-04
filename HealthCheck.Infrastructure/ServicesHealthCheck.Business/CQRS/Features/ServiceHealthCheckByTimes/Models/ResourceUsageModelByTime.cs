﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.CQRS.Features.ServiceHealthCheckByTimes.Models
{
    public class ResourceUsageModelByTime
    {
        public float CpuUsage { get; set; }
        public float PhysicalMemoryUsage { get; set; }
        public float VirtualMemoryUsage { get; set; }
        public float PrivateMemoryUsage { get; set; }
        public float DiskUsage { get; set; }
        public bool IsHealthy { get; set; }
        public DateTime Date { get; set; }
    }
}
