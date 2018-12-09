using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRServer.Hubs
{
    public class FlightSimulationHub : Hub
    {
        public async Task GetUpdateForStatus()
        {
            int i = 0;
            do
            {
                Thread.Sleep(5000);
                await Clients.All.SendAsync("ReceiveUpdateForStatus", i);
            } while (i < 5);
            await Clients.Caller.SendAsync("Finished");
        }

        public override async Task OnConnectedAsync()
        {
            var connectioonId = Context.ConnectionId;

            await Clients.AllExcept(connectioonId).SendAsync("NewFlight");
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
