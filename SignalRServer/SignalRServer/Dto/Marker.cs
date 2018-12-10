using System.Collections.Generic;

namespace SignalRServer.Dto
{
    public class FlightData
    {
        public int orderId { get; set; }
        public string connectionId { get; set; }
        public string personName { get; set; }
        public string routeName { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }


}
