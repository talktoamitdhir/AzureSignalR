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
            do
            {
                Thread.Sleep(1000);
                var nextFlightData = Data.LocationDetails.LAX_DELHI.SingleOrDefault(s => s.orderId == i);
                nextFlightData.personName = personName;
                nextFlightData.routeName = routeName;
                nextFlightData.connectionId = Context.ConnectionId;
                await Clients.All.SendAsync("ReceiveUpdateForStatus", nextFlightData);
                i++;
            } while (Data.LocationDetails.LAX_DELHI.FirstOrDefault(s => s.orderId == i) != null);

            Thread.Sleep(6000);
            await Clients.Caller.SendAsync("Finished");
        }

    }
}
