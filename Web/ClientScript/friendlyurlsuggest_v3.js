var keyCode;

function UrlHelper(inputText, outputText, referenceText, warningSpan, serviceUrl, urlPrefix = '')
{
    this.inputText = inputText;
	this.referenceText = referenceText;
	this.outputText = outputText;
	this.warningSpan = warningSpan;
	this.urlPrefix = urlPrefix;
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
    if (this.inputText.value === this.referenceText.value) { return; }
	this.query = this.inputText.value;
	//alert(this.query);
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

UrlHelper.prototype.ShowData = function(urlHelper, xmlhttp) {
    
	this.xmlDoc = xmlhttp;

    if (xmlDoc) {
		var nUrl = $(xmlDoc).find("fn").text();
        urlHelper.outputText.value = nUrl;
        var nWarning = $(xmlDoc).find("wn").text();
        var message = nWarning;
        if (message.length > 0) { urlHelper.warningSpan.innerHTML = message; urlHelper.warningSpan.style.display = 'inline'; }
		else { urlHelper.warningSpan.style.display = 'none'; }
    }
}

UrlHelper.prototype.showWarning = function (urlHelper,xmlhttp)
{
    this.xmlDoc = xmlhttp;

    if(xmlDoc)
    {
		var nWarning = $(xmlDoc).find("wn").text();
        var message = nWarning;
        if (message.length > 0) { urlHelper.warningSpan.innerHTML = message; urlHelper.warningSpan.style.display = 'inline';  }
		else { urlHelper.warningSpan.style.display = 'none'; }
    }

}

UrlHelper.prototype.GetData = function(callback,urlHelper)
{
	var separator = this.serviceUrl.indexOf('?') !== -1 ? "&" : "?";
	$.ajax({
	url: this.serviceUrl + separator + 'pn=' + encodeURIComponent(this.query) + '&prefix=' + encodeURIComponent(this.urlPrefix),
    dataType: 'xml',
    success: function(data){
		this.xmlDoc = data;
		callback(urlHelper, data);
    },
    error: function(data){
        console.log('Error loading XML data');
    }
	});
	
}

UrlHelper.prototype.CheckUrl = function(callback,urlHelper)
{
	var separator = this.serviceUrl.indexOf('?') !== -1 ? "&" : "?";
	$.ajax({
    url: this.serviceUrl + separator + 'cu=' + encodeURIComponent(this.urlcheckquery),
    dataType: 'xml',
    success: function(data){
		callback(urlHelper, data);
    },
    error: function(data){
        console.log('Error loading XML data');
    }
});
	
}

function http_onreadystatechange(sender,callback,urlHelper)
{
    if (sender.readyState === /* complete */ 4)
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
