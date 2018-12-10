using Microsoft.AspNetCore.SignalR;
using SignalRServer.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRServer.Hubs
{
    public class FlightSimulationHub : Hub
    {
        

        public FlightSimulationHub()
        {

        }

        public async Task GetUpdateForStatus(string personName, string routeName)
        {
            int i = 1;
            while (LAX_DELHI.SingleOrDefault(s => s.orderId == i) != null)
            {
                Thread.Sleep(1000);
                var nextFlightData = LAX_DELHI.SingleOrDefault(s => s.orderId == i);
                nextFlightData.personName = personName;
                nextFlightData.routeName = routeName;
                nextFlightData.connectionId = Context.ConnectionId;
                await Clients.All.SendAsync("ReceiveUpdateForStatus", nextFlightData);
                i++;
            }
            await Clients.Caller.SendAsync("Finished");
        }

    }
}
