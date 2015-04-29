
   
var map;

function initialize() {
    var mapOptions = {
        zoom: 19
    };
    map = new google.maps.Map(document.getElementById('map-canvas'),mapOptions);

    // Try HTML5 geolocation
    if(navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function(position) {
        
            var pos = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);

            var image = '/images/drone.png';

            var home = '/images/home.png';
         
            var shopping = '/images/shopping.png';
            var line;


           
          
            var homemarker = new google.maps.Marker({
                map: map,
                position: pos,
                icon: home,
            });
            


            var marker = new google.maps.Marker({
                map: map,
                icon: shopping
            });


            var mymarker = new google.maps.Marker({
                map: map,
          
                icon: image
            });
         

   
            google.maps.event.addListener(map, 'click', function (event) {
                placeMarker(event.latLng);
            });

            function placeMarker(location) {
                marker.setPosition(location);
                DronePath(map);
            }
            



                //Drone Path map 

                //google.maps.event.addListener(map, 'click', function (event) {

       





                function DronePath(map) {
               
                    //Coordinates
                   

                    var departure = marker.getPosition(); //Set to whatever lat/lng you need for your departure location
                    var arrival = pos; //Set to whatever lat/lng you need for your arrival location
                   
                  
                    var markerpos = new google.maps.Marker({
                        map: map,
                        position: arrival,
                        icon: home
                    });

    


                    //Line and circle
                    var lineSymbol = {
                        path: google.maps.SymbolPath.CIRCLE,
                        scale: 8,
                        strokeColor: '#393'
                    };
                   
                   var line = new google.maps.Polyline({
                        path: [departure, arrival],
                        icons: [{
                            icon: lineSymbol,
                            offset: '100%'
                        }],
                        strokeColor: "#FF0000",
                        strokeOpacity: 1,
                        strokeWeight: 3,
                        geodesic: true, //set to false if you want straight line instead of arc
                        map: map
                    });

                    //call animate 
                    animateCircle();

                    var step = 0;
                    var numSteps = 950; //Change this to set animation resolution
                    var timePerStep = 7; //Change this to alter animation speed
                    var interval = setInterval(function () {
                        step += 1;
                        if (step > numSteps) {
                            clearInterval(interval);
                        } else {
                            var are_we_there_yet = google.maps.geometry.spherical.interpolate(departure, arrival, step / numSteps);


                            mymarker.setPosition(are_we_there_yet);

                        }
                    }, timePerStep);

                }

                var panelDiv = document.getElementById('panel');

                var data = new PlacesDataSource(map);



                var view = new storeLocator.View(map, data);

                var markerSize = new google.maps.Size(32, 32);
                var storemarker;
                view.createMarker = function (store) {
                    
                    var storemarker = new google.maps.Marker({
                        position: store.getLocation(),
                        icon: new google.maps.MarkerImage(store.getDetails().icon, null, null,
                            null, markerSize),
                        //new Drone(store.)
                    });
                    return storemarker;
                };

                new storeLocator.Panel(panelDiv, {
                    view: view
                });
            

            /**
             * Creates a new PlacesDataSource.
             * @param {google.maps.Map} map
             * @constructor
             */


            function PlacesDataSource(map) {
                this.service_ = new google.maps.places.PlacesService(map);

            }

            /**
             * @inheritDoc
             */

            function animateCircle() {
                var count = 0;
                window.setInterval(function () {
                    count = (count + 1) % 200;

                    var icons = line.get('icons');
                    icons[0].offset = (count / 2) + '%';
                    line.set('icons', icons);
                }, 20);
            }



            //PlaceDatasource 
            PlacesDataSource.prototype.getStores = function (bounds, features, callback) {
                this.service_.search({
                    bounds: bounds,
                    types: ['grocery_or_supermarket']
                }, function (results, status) {
                    var stores = [];

                    for (var i = 0, result; result = results[i]; i++) {
                        var latLng = result.geometry.location;
                        var store = new storeLocator.Store(result.id, latLng, null, {
                            title: result.name,
                            address: result.vicinity,
                            icon: result.icon
                        });
                        stores.push(store);
                    }

                    callback(stores);
                });
            }


        }, function() {
            handleNoGeolocation(true);
        });
    } else {
        // Browser doesn't support Geolocation
        handleNoGeolocation(false);
    }




   
}

//no geolocation
function handleNoGeolocation(errorFlag) {
    if (errorFlag) {
        var content = 'Error: The Geolocation service failed.';
    } else {
        var content = 'Error: Your browser doesn\'t support geolocation.';
    }

    var options = {
        map: map,
        position: new google.maps.LatLng(60, 105),
        content: content
    };

    var infowindow = new google.maps.InfoWindow(options);
    map.setCenter(options.position);

}





//event listener 
google.maps.event.addDomListener(window, 'load', initialize);

  


