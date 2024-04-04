using Microsoft.AspNetCore.SignalR;

namespace ServicesHealthCheck.SignalR.Hubs
{
    public class HealthCheckHub : Hub
    {
        public async Task SendMessageAsync(string serviceName,string status,string cpuUsage,string physicalMemoryUsage,string virtualMemoryUsage,string privateMemoryUsage,bool isHealthy)
        {
            await Clients.All.SendAsync("receiveMessage", serviceName, status, cpuUsage, physicalMemoryUsage,
                virtualMemoryUsage, privateMemoryUsage, isHealthy);
        }
    }
}
