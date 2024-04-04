using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using ServicesHealthCheck.Business.RealTimes.SignalR.Abstract;

namespace ServicesHealthCheck.Business.RealTimes.SignalR
{
    public class SignalRService : ISignalRService
    {
        private readonly HubConnection _hubConnection;

        public SignalRService()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5012/healthcheckhub")
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

        public async Task SendMessageAsync(string serviceName, string status, string cpuUsage, string physicalMemoryUsage,
            string virtualMemoryUsage, string privateMemoryUsage, bool isHealthy)
        {
            await StartConnectionAsync();
            await _hubConnection.InvokeAsync("SendMessageAsync", serviceName, status, cpuUsage, physicalMemoryUsage,
                virtualMemoryUsage, privateMemoryUsage, isHealthy);
        }
    }
}
