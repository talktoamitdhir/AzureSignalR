# AzureSignalR

This Project is to demostrate Azure SignalR Implementation
https://developers.google.com/maps/documentation/javascript/examples/overlay-symbol-animate
https://developers.google.com/maps/documentation/javascript/custom-markers
https://developers.google.com/maps/documentation/javascript/examples/overlay-popup

Db will have something like this

LAX_DELHI = 
{
  RouteId : 1,
  latlngs : [
    {
      locationId : 1,
      lat:124.541,
      lng:845.541
    },
    {
      locationId : 2,
      lat:124.541,
      lng:845.541
    },
  ]
}

1. Click on Go will send message to Notification hub with first latng 

Name : Amit
locationId : 1,
lat:124.541,
lng:845.541

2. Web Page will listen to SignalR notification and will draw plane on the map for Latlng.

3. Web Page will send next another send message to Notification hub with second latng and will remove plane from it's existing first lat lng.

  Name : Amit
  locationId : 2,
  lat:124.541,
  lng:845.541
  
4. If there's no next latlng, Plane will just disappear.

![Mock up](https://github.com/talktoamitdhir/learnangular/blob/master/AzureSignalR_MockUp.jpg)
