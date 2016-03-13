﻿var map;
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
function returnPremises(type){
    if(type==1){
        return '/Image/icon/clinic.png';
    }
    if(type==2){
        return '/Image/icon/dining.png';
    }
    if(type==3){
        return '/Image/icon/gym.png';
    }
    if(type==4){
        return '/Image/icon/highway.png';
    }
    if(type==5){
        return '/Image/icon/landed.png';
    }
    if(type==6){
        return '/Image/icon/park.png';
    }
    if(type==7){
        return '/Image/icon/parking.png';
    }
    if(type==8){
        return '/Image/icon/school.png';
    }
    if(type==9){
        return '/Image/icon/train.png';
    }
}
function InitializeMap() {

    var latlng = new google.maps.LatLng(1.373334, 103.835518);
    var myOptions = {
        zoom: 11,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
    ////createMarker();
    //@if (Model != null)
    //{
    //    <text>createPropertyMarker(@Html.DisplayFor(modeitem => Model.property.propertyID), "@Html.DisplayFor(model => model.property.address)", @Html.DisplayFor(model => model.property.Latitude), @Html.DisplayFor(model => model.property.Longitude), "@Html.DisplayFor(model => model.property.propertyType)");</text>
    //    //console.log("got model");
    //    foreach (var item in Model.listOfPremise)
    //    {
    //        <text>createPremisesMarker(@Html.DisplayFor(modeitem => item.premises_id), "@Html.DisplayFor(model => item.premises_address)", @Html.DisplayFor(model => item.premises_lat), @Html.DisplayFor(model => item.premises_long), "@Html.DisplayFor(model => item.premises_type_id)");</text>
    //    }
    //}


}
window.onload = InitializeMap;

function testHello() {
    window.alert("hello");
    document.getElementById("demo").innerHTML="hello";
    return true;
}
