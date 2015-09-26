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
	addLoadEvent(SetHover__AspNetMenu);
}
