using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesHealthCheck.Business.RealTimes.SignalR.Abstract
{
    public interface ISignalRService
    {
        Task SendMessageAsync(string serviceName,string status,string cpuUsage,string physicalMemoryUsage,string virtualMemoryUsage,string privateMemoryUsage,bool isHealthy);
    }
}
