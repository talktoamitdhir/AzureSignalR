using Microsoft.AspNetCore.SignalR;
using SignalRServer.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRServer.Hubs
{
    public class FlightSimulationHub : Hub
    {
        public async Task GetUpdateForStatus(string personName, string routeName)
        {
            switch (routeName)
            {
                case "LAX_DELHI":
                case "LAX_TOKYO":
                case "DELHI_TOKYO":
                    await InitiateFlightFromJsonFile(personName, routeName, "right");
                    break;
                case "TOKYO_LAX":
                case "TOKYO_DELHI":
                case "DELHI_LAX":
                    await InitiateFlightFromJsonFile(personName, routeName, "left");
                    break;

                default:
                    // do nothing
                    break;
            }
        }

        private async Task InitiateFlightFromJsonFile(string personName, string routeName, string direction)
        {
            try
            {
                int sleepTimeInMs = 2000;
                string path = $"Data/{routeName.ToUpper()}_{direction.ToUpper()}.json";
                var Cordinates = File.ReadAllText(@path);
                var points = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FlightData>>(Cordinates);
                foreach (var nextFlightData in points)
                {
                    Thread.Sleep(sleepTimeInMs);
                    nextFlightData.personName = personName;
                    nextFlightData.routeName = routeName;
                    nextFlightData.connectionId = Context.ConnectionId;
                    nextFlightData.direction = direction;
                    await Clients.All.SendAsync("ReceiveUpdateForStatus", nextFlightData);
                }
                Thread.Sleep(sleepTimeInMs);
                await Clients.All.SendAsync("RemovePlane", Context.ConnectionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task InitiateFlight(string personName, string routeName, List<FlightData> flightDatas, string direction)
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
                nextFlightData.direction = direction;
                await Clients.All.SendAsync("ReceiveUpdateForStatus", nextFlightData);
                i++;
            } while (flightDatas.FirstOrDefault(s => s.orderId == i) != null);
            Thread.Sleep(sleepTimeInMs);
            await Clients.All.SendAsync("RemovePlane", Context.ConnectionId);
        }

    }
}
