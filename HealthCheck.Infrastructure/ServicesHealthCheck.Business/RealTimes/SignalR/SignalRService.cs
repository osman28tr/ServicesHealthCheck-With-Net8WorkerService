using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using ServicesHealthCheck.Business.RealTimes.SignalR.Abstract;
using ServicesHealthCheck.Dtos.ServiceHealthCheckDtos;
using ServicesHealthCheck.Dtos.SignalRDtos;

namespace ServicesHealthCheck.Business.RealTimes.SignalR
{
    public class SignalRService : ISignalRService
    {
        private readonly HubConnection _hubConnection;

        public SignalRService()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5181/healthcheckhub")
                .Build();
        }
        private async Task StartConnectionAsync()
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync();
            }

            return;
        }

        public async Task SendMessageAsync(ServicesHealthCheckSignalRDto servicesHealthCheckSignalRDto)
        {
            await StartConnectionAsync();
            await _hubConnection.InvokeAsync("SendMessageAsync", servicesHealthCheckSignalRDto);
        }

        public async Task SendVisualizationMessageAsync(ServiceResourceUsageVisualizationSignalRDto serviceResourceUsageVisualizationSignalRDto)
        {
            await StartConnectionAsync();
            await _hubConnection.InvokeAsync("SendVisualizationMessageAsync", serviceResourceUsageVisualizationSignalRDto);
        }
    }
}
