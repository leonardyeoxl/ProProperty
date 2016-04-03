var map;
var markers = {};
var googleMap =
{
    init: function () {
        var latlng = new google.maps.LatLng(1.373334, 103.835518);
        var myOptions = {
            zoom: 11,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        map = new google.maps.Map(document.getElementById("map_canvas"), myOptions)
    }
}
var Property =
{
    _markerId: '',
    _markerAddress: '',
    _markerLat: '',
    _markerLong: '',
    _markerType: '',
    Create: function (markerId, markerAddress, markerLat, markerLong, marketType) {
        var image = returnIcon(marketType);
        createPropertyMarker(markerId, markerAddress, markerLat, markerLong, marketType);
    }
}
function createPropertyMarker(id, address, lat, lng, type) {
    console.log(id + lat);
    console.log(lng);
    console.log(type);
    var image = returnIcon(type);
    console.log(image);
    var marker = new google.maps.Marker
    (
        {
            position: new google.maps.LatLng(lat, lng),
            map: map,
            icon: image,
            title: address
        }
    );
    var cityCircle = new google.maps.Circle({
        strokeColor: '#00ff7f',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#00ff7f',
        fillOpacity: 0.35,
        map: map,
        center: new google.maps.LatLng(lat, lng),
        radius: 1600
    });
    //for clicking on the marker
    var infowindow = new google.maps.InfoWindow({
        content: address
    });
    google.maps.event.addListener(marker, 'click', function () {
        infowindow.open(map, marker);
    });
}
var Premise =
{
    _markerId: '',
    _markerAddress: '',
    _markerLat: '',
    _markerLong: '',
    _markerType: '',
    Create: function (markerId, markerAddress, markerLat, markerLong, marketType) {
        var image = returnPremises(marketType);
        createPremisesMarker(markerId, markerAddress, markerLat, markerLong, marketType);
    }
}
function createPremisesMarker(id, address, lat, lng, type) {
    console.log(id + lat);
    console.log(lng);
    console.log(type);
    var image = returnPremises(type);
    console.log(image);
    var marker = new google.maps.Marker
        (
            {
                position: new google.maps.LatLng(lat, lng),
                map: map,
                icon: image,
                title: address
            }
        );
    markers[id + ''] = marker;
    //for clicking on the marker
    var infowindow = new google.maps.InfoWindow({
        content: address
    });
    google.maps.event.addListener(marker, 'click', function () {
        infowindow.open(map, marker);
    });
}
function returnIcon(type) {
    if (type == "hdb") {
        return '/Image/icon/hdb.png';
    }
    if (type == "private") {
        return '/Image/icon/private.png';
    }
    if (type == "landed") {
        return '/Image/icon/landed.png';
    }
}
function returnPremises(type) {
    if (type == 1) {
        return '/Image/icon/supermarket.jpg';
    }
    if (type == 2) {
        return '/Image/icon/dining.png';
    }
    if (type == 3) {
        return '/Image/icon/train.png';
    }
    if (type == 4) {
        return '/Image/icon/bus.png';
    }
    if (type == 5) {
        return '/Image/icon/parking.png';
    }
    if (type == 6) {
        return '/Image/icon/clinic.png';
    }
    if (type == 7) {
        return '/Image/icon/park.png';
    }
    if (type == 8) {
        return '/Image/icon/train.png';
    }
    if (type == 9) {
        return '/Image/icon/cc.png';
    }
    if (type == 10) {
        return '/Image/icon/school.png';
    }
}
function showHidePremiseMarkers(id) {
    var marker = markers[id + ''];
    if (!marker.getVisible()) {
        marker.setVisible(true);
    }
    else {
        marker.setVisible(false);
    }
}