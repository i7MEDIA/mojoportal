var geocoder = null;

function initializemojomaputils() {
if (GBrowserIsCompatible()) {
	geocoder = new GClientGeocoder();
	}
}

function showGMap(divMap, address, enableMapType, enableZoom, showInfoWindow, enableLocalSearch, mapType, zoomLevel) 
{
	if (geocoder == null) 
	{
	 initializemojomaputils();
	}
	
	if (geocoder) 
	{
	    geocoder.getLatLng(
	  address,
	  function(point) {
	      if (!point) {
	          //alert(address + " not found");
	          divMap.innerHTML = 'location not found';
	      }
	      else {
	          var map = new GMap2(divMap);
	          
	          map.setCenter(point, 13);

	          if (mapType)
	              map.setMapType(mapType);

	          var marker = new GMarker(point);
	          map.addOverlay(marker);

	          if (enableZoom)
	              map.addControl(new GLargeMapControl());

	          if (enableMapType)
	              map.addControl(new GMapTypeControl());

	          if (showInfoWindow)
	              marker.openInfoWindowHtml(address);

	          if (enableLocalSearch) {
	              map.addControl(new google.maps.LocalSearch(), new GControlPosition(G_ANCHOR_BOTTOM_RIGHT, new GSize(10, 20)));
	          }

	          if (zoomLevel)
	              map.setZoom(zoomLevel);
	          //alert(map.getZoom());

	      }
	  }
	);
	}
}

function showMapAndDirections(divMapId, divDirectionsId, txtFromAddressId, toAddress, locale) 
{
    if (!GBrowserIsCompatible()){return;}
    
    var map = new GMap2(document.getElementById(divMapId));
    
	if(! map){ return;}
	
	var divDir = document.getElementById(divDirectionsId);
	if(! divDir){ alert('divdir was null');return;}
	
	var gdir = new GDirections(map, divDir);
	
	var txtFrom = document.getElementById(txtFromAddressId);
	if(! txtFrom){ return;}
    
    var fromLocation = txtFrom.value;
	
	gdir.load("from: " + fromLocation + " to: " + toAddress,{ "locale": locale });
	  
}
    
