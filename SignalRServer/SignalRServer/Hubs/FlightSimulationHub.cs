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
            int sleepTimeInMs = 1000;
            int i = 1;
            do
            {
                Thread.Sleep(sleepTimeInMs);
                var nextFlightData = Data.LocationDetails.LAX_DELHI.SingleOrDefault(s => s.orderId == i);
                nextFlightData.personName = personName;
                nextFlightData.routeName = routeName;
                nextFlightData.connectionId = Context.ConnectionId;
                await Clients.All.SendAsync("ReceiveUpdateForStatus", nextFlightData);
                i++;
            } while (Data.LocationDetails.LAX_DELHI.FirstOrDefault(s => s.orderId == i) != null);
            Thread.Sleep(sleepTimeInMs);
            await Clients.All.SendAsync("Finished", Context.ConnectionId);
        }

    }
}
