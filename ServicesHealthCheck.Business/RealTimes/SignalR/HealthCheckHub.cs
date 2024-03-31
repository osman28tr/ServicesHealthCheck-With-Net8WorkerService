using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ServicesHealthCheck.Business.RealTimes.SignalR
{
    public class HealthCheckHub : Hub
    {
        public async Task SendMessage(string serviceName, string status)
        {
            await Clients.All.SendAsync("ReceiveMessage", serviceName, status);
        }
    }
}
