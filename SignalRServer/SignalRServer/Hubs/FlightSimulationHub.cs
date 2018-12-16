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
            switch (routeName)
            {
                case "LAX_DELHI":
                    await InitiateFlight(personName, routeName, Data.LocationDetails.LAX_DELHI);
                    break;
                case "LAX_TOKYO":
                    await InitiateFlight(personName, routeName, Data.LocationDetails.LAX_TOKYO);
                    break;
                default:
                    // do nothing
                    break;
            }
        }

        private async Task InitiateFlight(string personName, string routeName, List<FlightData> flightDatas)
        {
            int sleepTimeInMs = 2000;
            int i = 1;
            do
            {
                Thread.Sleep(sleepTimeInMs);
                var nextFlightData = flightDatas.SingleOrDefault(s => s.orderId == i);
                nextFlightData.personName = personName;
                nextFlightData.routeName = routeName;
                nextFlightData.connectionId = Context.ConnectionId;
                await Clients.All.SendAsync("ReceiveUpdateForStatus", nextFlightData);
                i++;
            } while (flightDatas.FirstOrDefault(s => s.orderId == i) != null);
            Thread.Sleep(sleepTimeInMs);
            await Clients.All.SendAsync("RemovePlane", Context.ConnectionId);
        }
    }
}
