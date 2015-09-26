var keyCode;

function UrlHelper(inputText, outputText, warningSpan, serviceUrl)
{
    this.inputText = inputText;
    this.outputText = outputText;
    this.warningSpan = warningSpan;
    this.query = '';
    this.urlcheckquery = '';
    this.xmlDoc;
    this.serviceUrl = serviceUrl;
    this.inputText.UrlHelper = this;
    this.outputText.UrlHelper = this;
    //this.inputText.onkeyup = UrlHelper.prototype.onKeyUp;
    this.inputText.onblur = UrlHelper.prototype.onblur;
    //this.outputText.onkeyup = UrlHelper.prototype.onUrlKeyUp;
    this.outputText.onblur = UrlHelper.prototype.onUrlBlur;

}

 UrlHelper.prototype.onblur = function()
 {
    //alert('you blurred');
    this.UrlHelper.onchange();
 }

UrlHelper.prototype.onKeyUp = function()
{
    this.UrlHelper.onchange();
}

UrlHelper.prototype.onchange = function()
{
	this.query = this.inputText.value;
	this.GetData(this.ShowData, this);
}

UrlHelper.prototype.onUrlKeyUp = function()
{
    
    this.UrlHelper.onurlchange();
}

UrlHelper.prototype.onUrlBlur = function()
{
    
    this.UrlHelper.onurlchange();
}

UrlHelper.prototype.onurlchange = function()
{
    this.urlcheckquery = this.outputText.value;
	this.CheckUrl(this.showWarning, this);
}

UrlHelper.prototype.ShowData = function (urlHelper,xmlhttp)
{
    this.xmlDoc = xmlhttp.responseXML;

    if(xmlDoc)
    {
	    if(document.all)
	    {
		    this.xmlDoc.setProperty('SelectionLanguage', 'XPath');
	    }

	    var nUrl = this.xmlDoc.selectSingleNode('//fn');
        urlHelper.outputText.value = Sarissa.getText(nUrl, true);
        var nWarning = this.xmlDoc.selectSingleNode('//wn');
        urlHelper.warningSpan.innerHTML = Sarissa.getText(nWarning, true);
    }
 }

UrlHelper.prototype.showWarning = function (urlHelper,xmlhttp)
{
    this.xmlDoc = xmlhttp.responseXML;

    if(xmlDoc)
    {
	    if(document.all)
	    {
		    this.xmlDoc.setProperty('SelectionLanguage', 'XPath');
	    }

        var nWarning = this.xmlDoc.selectSingleNode('//wn');
        urlHelper.warningSpan.innerHTML = Sarissa.getText(nWarning, true);
    }

}

UrlHelper.prototype.GetData = function(callback,urlHelper)
{
	var xmlhttp = new XMLHttpRequest();
	xmlhttp.open('GET', this.serviceUrl + '?pn=' + encodeURIComponent(this.query), true);
	
	xmlhttp.onreadystatechange = function() 
	{ http_onreadystatechange(xmlhttp, callback,urlHelper); }
	
	xmlhttp.send(null);
	this.xmlDoc = xmlhttp.responseXML;
	
}

UrlHelper.prototype.CheckUrl = function(callback,urlHelper)
{
    
	var xmlhttp = new XMLHttpRequest();
	xmlhttp.open('GET', this.serviceUrl + '?cu=' + encodeURIComponent(this.urlcheckquery), true);
	xmlhttp.onreadystatechange = function() 
	{ http_onreadystatechange(xmlhttp, callback, urlHelper); }
	
	xmlhttp.send(null);
	
}

function http_onreadystatechange(sender,callback,urlHelper)
{
    if (sender.readyState == /* complete */ 4)
    { 
        callback(urlHelper, sender);
    }
}




function whichKey(e) {
if (document.all) {
	keyCode=event.keyCode;
	}
else {
	keyCode= e.which;
	}

}

document.onkeydown = whichKey;
