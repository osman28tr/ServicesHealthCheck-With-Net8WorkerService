using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;
using ServicesHealthCheck.Dtos.SignalRDtos;

namespace ServicesHealthCheck.Business.RealTimes.SignalR.Abstract
{
    public interface ISignalRService
    {
        Task SendMessageAsync(ServicesHealthCheckSignalRDto servicesHealthCheckSignalRDto);
        Task SendVisualizationMessageAsync(ServiceResourceUsageVisualizationSignalRDto serviceResourceUsageVisualizationSignalRDto);
    }
}
