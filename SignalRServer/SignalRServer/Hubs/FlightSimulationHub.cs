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
        private static List<FlightData> LAX_DELHI = new List<FlightData>() {
            new FlightData(){
                orderId = 1,
                lat = 36.2449313,
                lng = -113.7316141
            },
            new FlightData(){
                orderId = 2,
                lat = 31.6347485,
                lng = -8.0778939
            },
        };

        public FlightSimulationHub()
        {

        }

        public async Task GetUpdateForStatus()
        {
            int i = 1;
            do
            {
                Thread.Sleep(5000);
                await Clients.All.SendAsync("ReceiveUpdateForStatus", LAX_DELHI.SingleOrDefault(s => s.orderId == i));
                i++;
            } while (LAX_DELHI.SingleOrDefault(s => s.orderId == i) != null);
            await Clients.Caller.SendAsync("Finished");
        }

    }
}
