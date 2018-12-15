var connection = null;
var map;
var markers = [];
var markers_array = [];
var signalRHubUrl = "https://signalr-monthly.azurewebsites.net/flightsimulationhub";
var mapCenterLocation = {lat:31.6347485, lng:-8.0778939};
var icons = {
    greenPlane : {
        icon : 'http://aux2.iconspalace.com/uploads/plane-icon-256.png'
    },
    blackPlane : {
        icon : 'http://www.download82.com/images/produse/iconuri/planeplotter.gif'
    },
    whitePlane : {
        icon : 'https://cdn2.iconfinder.com/data/icons/fatcow/32x32/plane.png'
    }
};

document.getElementById("submit").addEventListener("click", e=>{
    e.preventDefault();
    const personName = document.getElementById("personName").value;
    connection.invoke("GetUpdateForStatus",personName,"LAX_DELHI");
});
    
function initMap() {        
    map = new google.maps.Map(document.getElementById('map'), {
        center: mapCenterLocation,
        zoom: 2.4,
        gestureHandling: "none",
        zoomControl: false
    });
}

setupConnection = () => {

    connection = new signalR.HubConnectionBuilder()
    .withUrl(signalRHubUrl)
    .build();

    connection.on("ReceiveUpdateForStatus",(update)=>{
        addUpdateConnectionIdData(update);
        placeAllMarkers();
    });

    connection.on("NewFlight",(update)=>{
        alert(update);
    });
    
    connection.on("Finished",(connectionId)=>{
        removeFromMarkers(connectionId);
        placeAllMarkers();
        connection.stop();
    });
    
    connection.start()
        .catch(err => console.error(err.toString()));

};

function placeMarker(lat,lng){

    var newLatLng = {lat:lat, lng:lng};

    var marker = new google.maps.Marker({
        position:newLatLng,
        map:map,
        title:name,
        id:id,
        icon:icons["whitePlane"].icon
    });    

    marker.setMap(map);
}

function addUpdateConnectionIdData(data){
    //console.log(markers);
    //console.log(data.connectionId);
    //console.log(markers.some(item => item.connectionId === data.connectionId));
    if(markers.some(item => item.connectionId === data.connectionId))
    {
        var currentMarker = markers.find(item => item.connectionId === data.connectionId);
        currentMarker.lat = data.lat;
        currentMarker.lng = data.lng;
    }else{
        markers.push(data);
    }
}

function placeAllMarkers(){

    for (var i = 0; i < markers_array.length; i++) {
        markers_array[i].setMap(null);
      }

    markers.forEach(function(feature) {
        var marker = new google.maps.Marker({
          position: {lat:feature.lat, lng:feature.lng},
          icon:icons["whitePlane"].icon,
          map: map,
          title:feature.personName,
          id:feature.connectionId
        });

        markers_array.push(marker);

      });
}

function removeFromMarkers(connectionId){
    var currentMarker = markers.findIndex(item => item.connectionId === connectionId);    
    markers.splice(currentMarker,1);
}

setupConnection();