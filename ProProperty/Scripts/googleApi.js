﻿var Story =
{
    _urlFormat: '',
    _urlHello: '',

    init: function (urlFormat,urlHello) {
        Story._urlFormat = urlFormat;
        window.alert(urlHello);
        window.alert(urlFormat);
        function InitializeMap() {

            var latlng = new google.maps.LatLng(1.373334, 103.835518);
            var myOptions = {
                zoom: 5,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions)
        }
        window.onload = InitializeMap;
    }
}

var Marker =
{
    _markerId:'',
    _markerAddress: '',
    _markerLat:'',
    _markerLong:'',
    _markerType: '',
    _LinkPropertyDetails:'',
    _LinkPropertyInformation:'',
    Create: function (map,markerId, markerAddress, markerLat, markerLong, marketType, LinkPropertyDetails, LinkPropertyInformation)
    {
        var image = returnIcon(marketType);
        createMarker(map,markerId, markerAddress, markerLat, markerLong, marketType, LinkPropertyDetails, LinkPropertyInformation);
    }
}

//var map;
function createMarker(map,id, address, lat, lng, type, LinkPropertyDetails, LinkPropertyInformation) {
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
    //for clicking on the marker
    var infowindow = new google.maps.InfoWindow({
        content: address + '</br><a href="' + LinkPropertyDetails + '"> View property </a></br><a href="' + LinkPropertyInformation + '"> View Information </a>'
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

