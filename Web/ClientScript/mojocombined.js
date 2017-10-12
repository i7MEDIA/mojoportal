function watermarkEnter(obj, wm) {
	if (obj.value == wm) {
	    obj.value = '';
		obj.style.color = '';
		obj.select();
	}
	
}
function watermarkLeave(obj, wm) {
	if (obj.value == '') {
		obj.value = wm;
      
	}
}
function CanHaveClass__CssFriendlyAdapters(element)
{
    return ((element != null) && (element.className != null));
}

function HasAnyClass__CssFriendlyAdapters(element)
{
    return (CanHaveClass__CssFriendlyAdapters(element) && (element.className.length > 0));
}

function HasClass__CssFriendlyAdapters(element, specificClass)
{
    return (HasAnyClass__CssFriendlyAdapters(element) && (element.className.indexOf(specificClass) > -1));
}

function AddClass__CssFriendlyAdapters(element, classToAdd)
{
    if (HasAnyClass__CssFriendlyAdapters(element))
    {
        if (!HasClass__CssFriendlyAdapters(element, classToAdd))
        {
            element.className = element.className + " " + classToAdd;
        }
    }
    else if (CanHaveClass__CssFriendlyAdapters(element))
    {
        element.className = classToAdd;
    }
}

function AddClassUpward__CssFriendlyAdapters(startElement, stopParentClass, classToAdd)
{
    var elementOrParent = startElement;
    while ((elementOrParent != null) && (!HasClass__CssFriendlyAdapters(elementOrParent, topmostClass)))
    {
        AddClass__CssFriendlyAdapters(elementOrParent, classToAdd);
        elementOrParent = elementOrParent.parentNode;
    }    
}

function SwapClass__CssFriendlyAdapters(element, oldClass, newClass)
{
    if (HasAnyClass__CssFriendlyAdapters(element))
    {
        element.className = element.className.replace(new RegExp(oldClass, "gi"), newClass);
    }
}

function SwapOrAddClass__CssFriendlyAdapters(element, oldClass, newClass)
{
    if (HasClass__CssFriendlyAdapters(element, oldClass))
    {
        SwapClass__CssFriendlyAdapters(element, oldClass, newClass);
    }
    else
    {
        AddClass__CssFriendlyAdapters(element, newClass);
    }
}

function RemoveClass__CssFriendlyAdapters(element, classToRemove)
{
    SwapClass__CssFriendlyAdapters(element, classToRemove, "");
}

function RemoveClassUpward__CssFriendlyAdapters(startElement, stopParentClass, classToRemove)
{
    var elementOrParent = startElement;
    while ((elementOrParent != null) && (!HasClass__CssFriendlyAdapters(elementOrParent, topmostClass)))
    {
        RemoveClass__CssFriendlyAdapters(elementOrParent, classToRemove);
        elementOrParent = elementOrParent.parentNode;
    }    
}

function IsEnterKey()
{
    var retVal = false;
    var keycode = 0;
    if ((typeof(window.event) != "undefined") && (window.event != null))
    {
        keycode = window.event.keyCode;
    }
    else if ((typeof(e) != "undefined") && (e != null))
    {
        keycode = e.which;
    }
    if (keycode == 13)
    {
        retVal = true;
    }
    return retVal;
}

var hoverClass = "AspNet-Menu-Hover";
var topmostClass = "AspNet-Menu";
var userAgent = navigator.userAgent;
var versionOffset = userAgent.indexOf("MSIE");
var isIE = (versionOffset >= 0);
var isPreIE7 = false;
var fullVersionIE = "";
var majorVersionIE = "";
if (isIE)
{
    fullVersionIE = parseFloat(userAgent.substring(versionOffset+5, userAgent.length));
    majorVersionIE = parseInt('' + fullVersionIE);
    isPreIE7 = majorVersionIE < 7;
}

function Hover__AspNetMenu(element)
{
    AddClass__CssFriendlyAdapters(element, hoverClass);

    if (isPreIE7)
    {
        var child = element.firstChild;
        while (child)
        {
            if (child.tagName == "UL")
            {
                var grandchild = child.firstChild;
                while (grandchild)
                {
                    if (grandchild.tagName == "LI")
                    {
                        if ((typeof(grandchild.iFrameFormElementMask) != "undefined") && (grandchild.iFrameFormElementMask != null))
                        {
                            grandchild.iFrameFormElementMask.style.display = "block";
                            
                            var w = grandchild.offsetWidth;
                            if ((grandchild.offsetWidth == 0) && (typeof(element.iFrameFormElementMask) != "undefined") && (element.iFrameFormElementMask != null) && (element.iFrameFormElementMask.style.width.length > 0))
                            {
                                w = element.iFrameFormElementMask.style.width;
                            }
                            grandchild.iFrameFormElementMask.style.width = w;
                            
                            var h = grandchild.offsetHeight + 5 /* fudge to cover margins between menu items */;
                            if ((grandchild.offsetHeight == 0) && (typeof(element.iFrameFormElementMask) != "undefined") && (element.iFrameFormElementMask != null) && (element.iFrameFormElementMask.style.height.length > 0))
                            {
                                h = element.iFrameFormElementMask.style.height;
                            }
                            grandchild.iFrameFormElementMask.style.height = h;
                        }
                    }
                    
                    grandchild = grandchild.nextSibling;
                }
            }

            child = child.nextSibling;
        }
    }
}

function Unhover__AspNetMenu(element)
{
    RemoveClass__CssFriendlyAdapters(element, hoverClass);

    if (isPreIE7)
    {
        var child = element.firstChild;
        while (child)
        {
            if (child.tagName == "UL")
            {
                var grandchild = child.firstChild;
                while (grandchild)
                {
                    if (grandchild.tagName == "LI")
                    {
                        if ((typeof(grandchild.iFrameFormElementMask) != "undefined") && (grandchild.iFrameFormElementMask != null))
                        {
                            grandchild.iFrameFormElementMask.style.display = "none";
                        }
                    }

                    grandchild = grandchild.nextSibling;
                }
            }

            child = child.nextSibling;
        }
    }
}

function SetHover__AspNetMenu()
{
    var menus = document.getElementsByTagName("ul");
    for (var i=0; i<menus.length; i++)
    {
        if(menus[i].className == topmostClass)
        {
            var items = menus[i].getElementsByTagName("li");
            for (var k=0; k<items.length; k++)
            {
                items[k].onmouseover = function() { Hover__AspNetMenu(this); }
                items[k].onmouseout = function() { Unhover__AspNetMenu(this); }
                
                if (isPreIE7 && ((typeof(items[k].iFrameFormElementMask) == "undefined") || (items[k].iFrameFormElementMask == null)))
                {
                    var iFrameFormElementMask = document.createElement("IFRAME");
                    iFrameFormElementMask.scrolling= "no";
                    iFrameFormElementMask.src = "javascript:false;";
                    iFrameFormElementMask.frameBorder = 0;
                    iFrameFormElementMask.style.display = "none";
                    iFrameFormElementMask.style.position = "absolute";
                    iFrameFormElementMask.style.filter = "progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=0)";

                    iFrameFormElementMask.style.zIndex = -1;
                    items[k].insertBefore(iFrameFormElementMask, items[k].firstChild);
                    items[k].iFrameFormElementMask = iFrameFormElementMask;
                }                
            }
        }
    }
}

if (isPreIE7)
{
    window.onload = SetHover__AspNetMenu;
}

var collapseClass = "AspNet-TreeView-Collapse";
var expandClass = "AspNet-TreeView-Expand";
var showClass = "AspNet-TreeView-Show";
var hideClass = "AspNet-TreeView-Hide";

function IsExpanded__AspNetTreeView(element)
{
    return (HasClass__CssFriendlyAdapters(element, collapseClass));
}

function TogglePlusMinus__AspNetTreeView(element, showPlus)
{
    if (HasAnyClass__CssFriendlyAdapters(element))
    {
        var showPlusLocal = IsExpanded__AspNetTreeView(element);
        if ((typeof(showPlus) != "undefined") && (showPlus != null))
        {
            showPlusLocal = showPlus;
        }    
        var oldClass = showPlusLocal ? collapseClass : expandClass;
        var newClass = showPlusLocal ? expandClass : collapseClass;
        SwapClass__CssFriendlyAdapters(element, oldClass, newClass);
    }
}

function ToggleChildrenDisplay__AspNetTreeView(element, collapse)
{
    if ((element != null) && (element.parentNode != null) && (element.parentNode.getElementsByTagName != null))
    {    
        var childrenToHide = element.parentNode.getElementsByTagName("ul");
        var oldClass = collapse ? showClass : hideClass;
        var newClass = collapse ? hideClass : showClass;
    	for (var i=0; i<childrenToHide.length; i++)
    	{
    	    if ((childrenToHide[i].parentNode != null) && (childrenToHide[i].parentNode == element.parentNode))
    	    {
        	    SwapOrAddClass__CssFriendlyAdapters(childrenToHide[i], oldClass, newClass);
        	}
		}
	}
}

function ExpandCollapse__AspNetTreeView(sourceElement)
{
    if (HasAnyClass__CssFriendlyAdapters(sourceElement))
    {
        var expanded = IsExpanded__AspNetTreeView(sourceElement);
        TogglePlusMinus__AspNetTreeView(sourceElement, expanded);
        ToggleChildrenDisplay__AspNetTreeView(sourceElement, expanded);
    }
}

function GetViewState__AspNetTreeView(id)
{
    var retStr = "";
    if ((typeof(id) != "undefined") && (id != null) && (document.getElementById(id) != null))
    {
        var topUL = document.getElementById(id);
        retStr = ComposeViewState__AspNetTreeView(topUL, "");        
    }
    return retStr;
}

function ComposeViewState__AspNetTreeView(element, state)
{
    var child = element.firstChild;
    var bConsiderChildren = true;

    //  The following line must be changed if you alter the TreeView adapters generation of
    //  markup such that the first child within the LI no longer is the expand/collapse <span>.
    if ((element.tagName == "LI") && (child != null))
    {
        var expandCollapseSPAN = null;
        var currentChild = child;
        while (currentChild != null)
        {
            if ((currentChild.tagName == "SPAN") &&
                (currentChild.className != null) &&
                ((currentChild.className.indexOf(collapseClass) > -1) ||
                 (currentChild.className.indexOf(expandClass) > -1)))
            {
                expandCollapseSPAN = currentChild;
                break;
            }
            currentChild = currentChild.nextSibling;
        }
    
        if (expandCollapseSPAN != null)
        {
            if (expandCollapseSPAN.className.indexOf(collapseClass) > -1)
            {
                //  If the "collapse" class is currently being used then the "collapse" icon is, presumably, being shown.
                //  In other words, the node itself is actually expanded at the present moment (which is why you now
                //  have the option of collapsing it.  So we mark it as an "expanded" node for purposes of the view state
                //  we are now accumulating.
                state +=  "e";
            }
            else if (expandCollapseSPAN.className.indexOf(expandClass) > -1)
            {
                //  This part of the tree is collapsed so we don't need to consider its children.
                bConsiderChildren = false;
                state +=  "n";
            }
        }
    }
        
    if (bConsiderChildren && (child != null))
    {
        state = ComposeViewState__AspNetTreeView(child, state);
    }
    
    if (element.nextSibling != null)
    {
        state = ComposeViewState__AspNetTreeView(element.nextSibling, state);
    }
    
    return state;
}


function trackUserLocation(location) {
    if (location == null) { return; }
    //alert(location.latitude);
    //alert(location.longitude);
    //alert(location.address.country_code);
    //alert(location.address.region);
    //.address.city
    //.address.country
    //TODO: call service and log it

}

/*
Script Name: Javascript Cookie Script
Author: Public Domain, with some modifications
Script Source URI: http://techpatterns.com/downloads/javascript_cookies.php
Version 1.1.2
Last Update: 5 November 2009
*/

// To use, simple do: Get_Cookie('cookie_name'); 
// replace cookie_name with the real cookie name, '' are required
function Get_Cookie( check_name ) {
	// first we'll split this cookie up into name/value pairs
	// note: document.cookie only returns name=value, not the other components
	var a_all_cookies = document.cookie.split( ';' );
	var a_temp_cookie = '';
	var cookie_name = '';
	var cookie_value = '';
	var b_cookie_found = false; // set boolean t/f default f
	var i = '';
	
	for ( i = 0; i < a_all_cookies.length; i++ )
	{
		// now we'll split apart each name=value pair
		a_temp_cookie = a_all_cookies[i].split( '=' );
		
		
		// and trim left/right whitespace while we're at it
		cookie_name = a_temp_cookie[0].replace(/^\s+|\s+$/g, '');
	
		// if the extracted name matches passed check_name
		if ( cookie_name == check_name )
		{
			b_cookie_found = true;
			// we need to handle case where cookie has no value but exists (no = sign, that is):
			if ( a_temp_cookie.length > 1 )
			{
				cookie_value = unescape( a_temp_cookie[1].replace(/^\s+|\s+$/g, '') );
			}
			// note that in cases where cookie is initialized but no value, null is returned
			return cookie_value;
			break;
		}
		a_temp_cookie = null;
		cookie_name = '';
	}
	if ( !b_cookie_found ) 
	{
		return null;
	}
}

/*
only the first 2 parameters are required, the cookie name, the cookie
value. Cookie time is in milliseconds, so the below expires will make the 
number you pass in the Set_Cookie function call the number of days the cookie
lasts, if you want it to be hours or minutes, just get rid of 24 and 60.

Generally you don't need to worry about domain, path or secure for most applications
so unless you need that, leave those parameters blank in the function call.
*/
function Set_Cookie( name, value, expires, path, domain, secure ) {
	// set time, it's in milliseconds
	var today = new Date();
	today.setTime( today.getTime() );
	// if the expires variable is set, make the correct expires time, the
	// current script below will set it for x number of days, to make it
	// for hours, delete * 24, for minutes, delete * 60 * 24
	if ( expires )
	{
		expires = expires * 1000 * 60 * 60 * 24;
	}
	//alert( 'today ' + today.toGMTString() );// this is for testing purpose only
	var expires_date = new Date( today.getTime() + (expires) );
	//alert('expires ' + expires_date.toGMTString());// this is for testing purposes only

	document.cookie = name + "=" +escape( value ) +
		( ( expires ) ? ";expires=" + expires_date.toGMTString() : "" ) + //expires.toGMTString()
		( ( path ) ? ";path=" + path : "" ) + 
		( ( domain ) ? ";domain=" + domain : "" ) +
		( ( secure ) ? ";secure" : "" );
}


/*
	Calls Set_Cookie but will always use root of site for path
*/
function Set_Root_Cookie(name, value, expires, domain, secure) {
	Set_Cookie(
		name,
		value,
		(expires) ? expires : "",
		"/",
		(domain) ? domain : "",
		(secure) ? secure : "");
}

// this deletes the cookie when called
function Delete_Cookie( name, path, domain ) {
	if ( Get_Cookie( name ) ) document.cookie = name + "=" +
			( ( path ) ? ";path=" + path : "") +
			( ( domain ) ? ";domain=" + domain : "" ) +
			";expires=Thu, 01-Jan-1970 00:00:01 GMT";
}

/* page exit prompt */
var exitThisPagePrompt = 'Are you sure you want to leave this page?';
var alreadySetExitPrompt = false;
var requireExitPrompt = false;
var contentDidChange = false;
function goodbye(e) {
    if (!requireExitPrompt) { return; }
    if (!contentDidChange) { return; }
    return exitThisPagePrompt;
}
function hookupGoodbyePrompt(message) {
    contentDidChange = true;
    if ((!alreadySetExitPrompt) && (requireExitPrompt)) {
        exitThisPagePrompt = message;
        alreadySetExitPrompt = true;
        window.onbeforeunload = goodbye;
    }
}
function unHookGoodbyePrompt() {
    requireExitPrompt = false;
}
