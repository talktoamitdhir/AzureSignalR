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
        public static bool isSenderRunning = false;

        public static int delay = 1000;

        private static List<FlightData> staticFlightData = new List<FlightData>();

        private static List<string> connectionsToBeRemoved = new List<string>();

        public async Task GetUpdateForStatus(string personName, string routeName)
        {
            await DeleteOldConnections();

            switch (routeName)
            {
                case "LAX_DELHI":
                case "LAX_TOKYO":
                case "DELHI_TOKYO":
                    await InitiateFlightFromJsonFileInOrder(personName, routeName, "right");
                    break;
                case "TOKYO_LAX":
                case "TOKYO_DELHI":
                case "DELHI_LAX":
                    await InitiateFlightFromJsonFileInOrder(personName, routeName, "left");
                    break;

                default:
                    // do nothing
                    break;
            }
        }

        private async Task InitiateFlightFromJsonFileInOrder(string personName, string routeName, string direction)
        {
            FlightData point = GetFirstPoint(routeName, direction);

            if (staticFlightData.Any(a => a.connectionId == Context.ConnectionId && a.direction == "none"))
            {
                var interestedItem = staticFlightData.First(a => a.connectionId == Context.ConnectionId);

                interestedItem.lat = point.lat;
                interestedItem.lng = point.lng;
                interestedItem.orderId = point.orderId;
                interestedItem.direction = direction;
                interestedItem.routeName = routeName;
            }
            else if (!staticFlightData.Any(a => a.connectionId == Context.ConnectionId))
            {

                staticFlightData.Add(new FlightData()
                {
                    connectionId = Context.ConnectionId,
                    direction = direction,
                    lat = point.lat,
                    lng = point.lng,
                    orderId = point.orderId,
                    personName = personName,
                    routeName = routeName
                });
            }

            if (!isSenderRunning)
            {
                await SendDataToAllClients();
            }
        }

        private async Task SendDataToAllClients()
        {
            isSenderRunning = true;

            while (staticFlightData.Count(c => c.direction != "none") != 0)
            {
                Thread.Sleep(delay);

                await Clients.All.SendAsync("SendDataToClient", staticFlightData);

                await UpdatestaticFlightData();
            }

            isSenderRunning = false;
        }

        private async Task UpdatestaticFlightData()
        {
            await Task.Factory.StartNew(() =>
            {
                SetNextFlightData();

            });

            await DeleteOldConnections();
        }

        private void SetNextFlightData()
        {
            foreach (var singleFlightData in staticFlightData)
            {
                var nextPoint = GetNextPoint(singleFlightData.routeName, singleFlightData.direction, singleFlightData.orderId);

                if (nextPoint != null)
                {
                    singleFlightData.orderId = nextPoint.orderId;
                    singleFlightData.lat = nextPoint.lat;
                    singleFlightData.lng = nextPoint.lng;
                }
                else
                {
                    if (connectionsToBeRemoved == null)
                    {
                        connectionsToBeRemoved = new List<string>();
                    }

                    connectionsToBeRemoved.Add(singleFlightData.connectionId);
                }
            }
        }

        private async Task DeleteOldConnections()
        {
            await Task.Factory.StartNew(async () =>
            {
                if (connectionsToBeRemoved != null)
                {
                    foreach (var itemToBeDeleted in connectionsToBeRemoved)
                    {
                        var item = staticFlightData.FirstOrDefault(f => f.connectionId == itemToBeDeleted);
                        item.direction = "none";
                    }

                    connectionsToBeRemoved = null;
                }
            });
        }

        private FlightData GetNextPoint(string routeName, string direction, int orderId)
        {
            if (direction != "none")
            {
                string path = $"Data/{routeName.ToUpper()}_{direction.ToUpper()}.json";
                var Cordinates = File.ReadAllText(@path);
                var point = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FlightData>>(Cordinates).FirstOrDefault(w => w.orderId == orderId + 1);
                return point;
            }
            else
            {
                return null;
            }
        }

        private static FlightData GetFirstPoint(string routeName, string direction)
        {
            string path = $"Data/{routeName.ToUpper()}_{direction.ToUpper()}.json";
            var Cordinates = File.ReadAllText(@path);
            var points = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FlightData>>(Cordinates).SingleOrDefault(w => w.orderId == 1);
            return points;
        }

        private async Task InitiateFlightFromJsonFile(string personName, string routeName, string direction)
        {
            try
            {
                string path = $"Data/{routeName.ToUpper()}_{direction.ToUpper()}.json";
                var Cordinates = File.ReadAllText(@path);
                var points = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FlightData>>(Cordinates);
                foreach (var nextFlightData in points)
                {
                    Thread.Sleep(delay);
                    nextFlightData.personName = personName;
                    nextFlightData.routeName = routeName;
                    nextFlightData.connectionId = Context.ConnectionId;
                    nextFlightData.direction = direction;
                    await Clients.All.SendAsync("ReceiveUpdateForStatus", nextFlightData);
                }
                Thread.Sleep(delay);
                await Clients.All.SendAsync("RemovePlane", Context.ConnectionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task InitiateFlight(string personName, string routeName, List<FlightData> flightDatas, string direction)
        {
            int i = 1;
            do
            {
                Thread.Sleep(delay);
                var nextFlightData = flightDatas.SingleOrDefault(s => s.orderId == i);
                nextFlightData.personName = personName;
                nextFlightData.routeName = routeName;
                nextFlightData.connectionId = Context.ConnectionId;
                nextFlightData.direction = direction;
                await Clients.All.SendAsync("ReceiveUpdateForStatus", nextFlightData);
                i++;
            } while (flightDatas.FirstOrDefault(s => s.orderId == i) != null);
            Thread.Sleep(delay);
            await Clients.All.SendAsync("RemovePlane", Context.ConnectionId);
        }

    }
}
