using SignalRServer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Data
{
    public static partial class LocationDetails
    {
        public static List<FlightData> LAX_DELHI = new List<FlightData>() {
            new FlightData(){
                orderId = 1,
                lat = 35.32774,
                lng = -119.70306
            },
            new FlightData(){
                orderId = 2,
                lat = 27.16994,
                lng = -65.09549
            },
            new FlightData(){
                orderId = 3,
                lat = 26.85674,
                lng = -14.47049
            },
            new FlightData(){
                orderId = 4,
                lat = 23.67806,
                lng = 37.56076
            },
            new FlightData(){
                orderId = 5,
                lat = 23.03255,
                lng = 75.52951
            }
        };

        public static List<FlightData> LAX_TOKYO = new List<FlightData>() {
            new FlightData(){
                orderId = 1,
                lat = 35.32774,
                lng = -119.70306
            },
            new FlightData(){
                orderId = 2,
                lat = 27.16994,
                lng = -65.09549
            },
            new FlightData(){
                orderId = 3,
                lat = 26.85674,
                lng = -14.47049
            },
            new FlightData(){
                orderId = 4,
                lat = 23.67806,
                lng = 37.56076
            },
            new FlightData(){
                orderId = 5,
                lat = 23.03255,
                lng = 75.52951
            },
            new FlightData(){
                orderId = 6,
                lat = 35.6895,
                lng = 139.6917
            }
        };
    }
}
