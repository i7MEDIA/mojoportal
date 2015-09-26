var geocoder=null;function initializemojomaputils(){geocoder=new google.maps.Geocoder();}
function mapTypeTrans(mapType)
{switch(mapType){case"G_NORMAL_MAP":return google.maps.MapTypeId.ROADMAP;break;case"G_HYBRID_MAP":return google.maps.MapTypeId.HYBRID;break;case"G_PHYSICAL_MAP":return google.maps.MapTypeId.TERRAIN;break;case"G_SATELLITE_MAP":return google.maps.MapTypeId.SATELLITE;break;default:return google.maps.MapTypeId.ROADMAP;break;}}
function showGMap(divMap,address,enableMapType,enableZoom,showInfoWindow,enableLocalSearch,mapType,zoomLevel)
{if(address.match(/^[+-]?\d+\.\d+, ?[+-]?\d+\.\d+$/))
{ var commaindex=address.indexOf(',',0)
latitude=address.substring(0,commaindex)
longitude=address.substring(commaindex+1,address.length+1)
var ll=new google.maps.LatLng(latitude,longitude);var mapOpts={zoomControl:enableZoom,zoom:zoomLevel,center:ll,mapTypeControl:enableMapType,mapTypeId:mapTypeTrans(mapType)};var map=new google.maps.Map(divMap,mapOpts);var marker=new google.maps.Marker({map:map,position:ll,title:address});if(showInfoWindow){var infoWindow=new google.maps.InfoWindow({content:address,position:ll});infoWindow.open(map,marker);}}
else
{if(geocoder==null)
{initializemojomaputils();}
if(geocoder)
{geocoder.geocode({'address':address},function(result,status){if(status==google.maps.GeocoderStatus.OK){var ll=new google.maps.LatLng();var mapOpts={zoomControl:enableZoom,zoom:zoomLevel,center:result[0].geometry.location,mapTypeControl:enableMapType,mapTypeId:mapTypeTrans(mapType)};var map=new google.maps.Map(divMap,mapOpts);var marker=new google.maps.Marker({map:map,position:result[0].geometry.location,title:address});if(showInfoWindow){var infoWindow=new google.maps.InfoWindow({content:address,position:result[0].geometry.location});infoWindow.open(map,marker);}}else{divMap.innerHTML='location not found';}});}else{divMap.innerHTML='service not available';}}}
function showMapAndDirections(divMapId,divDirectionsId,fromAddressId,toAddress,enableZoom,enableMapType,mapType)
{var directionsService=new google.maps.DirectionsService();var directionsDisplay=new google.maps.DirectionsRenderer();var charlotte=new google.maps.LatLng(35.2274,-80.8432);var mapOpts={zoomControl:enableZoom,center:charlotte,mapTypeControl:enableMapType,mapTypeId:mapTypeTrans(mapType)};var map=new google.maps.Map(document.getElementById(divMapId),mapOpts);directionsDisplay.setMap(map);document.getElementById(divDirectionsId).innerHTML='';directionsDisplay.setPanel(document.getElementById(divDirectionsId));var fromAddress=document.getElementById(fromAddressId).value;var request={origin:fromAddress,destination:toAddress,travelMode:google.maps.TravelMode.DRIVING};directionsService.route(request,function(result,status){if(status==google.maps.DirectionsStatus.OK){directionsDisplay.setDirections(result);}else{divMapId.innerHTML='directions call unsuccessful';}});}
