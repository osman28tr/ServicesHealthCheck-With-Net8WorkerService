using Microsoft.AspNetCore.SignalR;
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;
using ServicesHealthCheck.Dtos.SignalRDtos;

namespace ServicesHealthCheck.SignalR.Hubs
{
    public class HealthCheckHub : Hub
    {
        public async Task SendMessageAsync(ServicesHealthCheckSignalRDto servicesHealthCheckSignalRDto) => await Clients.All.SendAsync("receiveMessage", servicesHealthCheckSignalRDto);
    }
}
