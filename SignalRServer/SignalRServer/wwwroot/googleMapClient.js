//https://www.countryflags.io/in/shiny/16.png

var connection = null;
var map;
var markers = [];
var markers_array = [];

//var signalRHubUrl = "http://localhost:53537/flightsimulationhub";
var signalRHubUrl = "https://signalr-monthly.azurewebsites.net/flightsimulationhub";
var mapCenterLocation = { lat: 31.6347485, lng: -8.0778939 };

document.getElementById("submit").addEventListener("click", e => {
    e.preventDefault();
    const personName = document.getElementById("personName").value;
    const routeName = document.getElementById("startAddress").value + "_" + document.getElementById("destinationAddress").value;
    connection.invoke("GetUpdateForStatus", personName, routeName);
});

function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: mapCenterLocation,
        zoom: 2.4,
        gestureHandling: "none",
        zoomControl: false
    });

    placeJapanMarker();
    placeIndiaMarker();
    placeUsaMarker();
}

setupConnection = () => {

    connection = new signalR.HubConnectionBuilder()
        .withUrl(signalRHubUrl)
        .build();

    connection.on("ReceiveUpdateForStatus", (update) => {
        removeFromMarkers(update.connectionId);
        addUpdateConnectionIdData(update);
        placeAllMarkers(update.direction, update.connectionId);
    });

    connection.on("NewFlight", (update) => {
        alert(update);
    });

    connection.on("Finished", (connectionId) => {
        removeFromMarkers(connectionId);
        placeAllMarkers("left", connectionId);
        connection.stop();
    });

    connection.on("RemovePlane", (connectionId) => {
        removeFromMarkers(connectionId);
        placeAllMarkers("left", connectionId);
    });

    connection.on("SendDataToClient", (listOfMarkers) => {
        removeAllMarkers();
        setNewListOfMarkers(listOfMarkers);
        placeAllNewMarkers();
    });

    connection.start()
        .catch(err => console.error(err.toString()));

};

function setNewListOfMarkers(listOfMarkers) {
    markers = listOfMarkers;
}

function removeAllMarkers() {
    for (var i = 0; i < markers_array.length; i++) {
        markers_array[i].setMap(null);
    }
    marker = [];
    markers_array = [];
}

function placeMarker(lat, lng) {

    var newLatLng = { lat: lat, lng: lng };

    var marker = new google.maps.Marker({
        position: newLatLng,
        map: map,
        title: name,
        id: id,
        icon: icons["whitePlane"].icon
    });

    marker.setMap(map);
}

function addUpdateConnectionIdData(data) {
    if (markers.some(item => item.connectionId == data.connectionId)) {
        var currentMarker = markers.find(item => item.connectionId === data.connectionId);
        currentMarker.lat = data.lat;
        currentMarker.lng = data.lng;
    } else {
        markers.push(data);
    }
}

function placeAllNewMarkers() {

    var icon_direction;

    markers.forEach(function (feature) {

        if (feature.direction != "none") {

            if (feature.direction == "left") {
                icon_direction = "plane_left.png";
            } else if (feature.direction == "right") {
                icon_direction = "plane_right.png";
            }

            var marker = new google.maps.Marker({
                position: { lat: feature.lat, lng: feature.lng },
                icon: icon_direction,
                map: map,
                title: feature.personName,
                id: feature.connectionId
            });

            icon_direction = "";

            markers_array.push(marker);

        }
        
    });
}

function placeAllMarkers(direction, connectionId) {
    var icon_direction;

    if (direction == "left") {
        icon_direction = "plane_left.png";
    } else if (direction == "right") {
        icon_direction = "plane_right.png";
    }

    markers.forEach(function (feature) {
        var marker = new google.maps.Marker({
            position: { lat: feature.lat, lng: feature.lng },
            icon: icon_direction,
            map: map,
            title: feature.personName,
            id: feature.connectionId
        });

        markers_array.push(marker);

    });
}

function removeFromMarkers(connectionId) {
    var currentMarker = markers.findIndex(item => item.connectionId == connectionId);
    markers.splice(currentMarker, 1);

    var currentMarker_array = markers_array.findIndex(item => item.id == connectionId);
    for (var i = 0; i < markers_array.length; i++) {
        if (markers_array[i].id == connectionId) {
            markers_array[i].setMap(null);
        }
    }
    markers_array.splice(currentMarker_array, 1);
}

function placeJapanMarker() {
    var marker = new google.maps.Marker({
        position: { lat: 36.2048, lng: 138.2529 },
        icon: "japanFlag.png",
        map: map,
        title: 'Japan'
    });
}

function placeIndiaMarker() {
    var marker = new google.maps.Marker({
        position: { lat: 20.5937, lng: 78.9629 },
        icon: "indiaFlag.png",
        map: map,
        title: 'India'
    });
}

function placeUsaMarker() {
    var marker = new google.maps.Marker({
        position: { lat: 37.0902, lng: -119.70306 },
        icon: "usaFlag.png",
        map: map,
        title: 'United States'
    });
}

setupConnection();