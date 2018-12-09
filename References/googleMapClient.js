var map;
    var name = "Amit Dhir";
    var id =  "Amit_Dhir_1";
    var mapCenterLocation = {lat:31.6347485, lng:-8.0778939};
    var newLatLng = {lat:36.2449313, lng:-113.7316141};
    var planeColor = "whitePlane";
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

    function goClicked(){
      alert(document.getElementById('name').innerHTML);
    }
    
    function initMap() {        
        map = new google.maps.Map(document.getElementById('map'), {
          center: mapCenterLocation,
          zoom: 2.4,
          gestureHandling: "none",
          zoomControl: false
        });

        var marker = new google.maps.Marker({
            position:newLatLng,
            map:map,
            title:name,
            id:id,
            icon:icons["blackPlane"].icon
        });

        marker.setMap(null);

        marker.setMap(map);
    }