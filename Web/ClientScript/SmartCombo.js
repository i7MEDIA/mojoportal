/*
	Author:			
	Created:			6/15/2005
	Last Modified:	9/25/2005


*/

var keyCode;

function SmartCombo(oText, oHidden, oDiv, oButton, oDropDown, Url)
{
	this.oText = oText;
	this.oHidden = oHidden;
	this.oDiv = oDiv;
	this.oButton = oButton
	this.query = "";
	this.xmlDoc;
	this.oUrl = Url;
	this.movingToDropDown = false;
	
	this.oDropDown = oDropDown;
	this.oDropDown.SmartCombo = this;
	this.oDropDown.onchange = SmartCombo.prototype.onDropDownChange;
	this.oDropDown.onblur = SmartCombo.prototype.onDropDownBlur;
	this.oDropDown.onclick = SmartCombo.prototype.onDropDownClick;
	
	this.oText.SmartCombo = this;
	this.oText.onkeyup = SmartCombo.prototype.onKeyUp;
	this.oText.onblur = SmartCombo.prototype.onblur;
	this.oButton.SmartCombo = this;
	this.oButton.onclick = SmartCombo.prototype.ButtonClick;
}

SmartCombo.prototype.onblur = function()
{
	if(this.SmartCombo.movingToDropDown)
	{
		this.SmartCombo.movingToDropDown = false;
	}
	else
	{
		if(this.SmartCombo.oText.value == "")
		{
			if(this.SmartCombo.oDropDown.selectedIndex == -1)
			{
				this.SmartCombo.oDiv.style.visibility = "hidden";
				this.SmartCombo.oDropDown.style.visibility = "hidden";
			}
		}
		else
		{
			this.SmartCombo.oDiv.style.visibility = "hidden";
			this.SmartCombo.oDropDown.style.visibility = "hidden";
		
		}
	}
		
}

SmartCombo.prototype.onDropDownBlur = function()
{
	this.SmartCombo.oDiv.style.visibility = "hidden";
	this.SmartCombo.oDropDown.style.visibility = "hidden";	
}


SmartCombo.prototype.onKeyUp = function()
{
	this.SmartCombo.onchange();
}

SmartCombo.prototype.onDropDownChange = function()
{	
	this.SmartCombo.movingToDropDown = true;
	this.SmartCombo.oText.value = this.options[this.selectedIndex].text;
	this.SmartCombo.oHidden.value = this.options[this.selectedIndex].value;	
}

SmartCombo.prototype.onDropDownClick = function()
{	
	this.SmartCombo.movingToDropDown = true;
}

SmartCombo.prototype.ButtonClick = function()
{
	this.SmartCombo.query = this.SmartCombo.oText.value;
	this.SmartCombo.GetData();

	return false;
}

SmartCombo.prototype.onchange = function()
{
	// if down arrow key set focus to dropdown
	//alert(keyCode);
	if(keyCode == 40)
	{
		if(this.oDropDown.hasChildNodes())
		{
			this.movingToDropDown = true;
			this.oDropDown.focus();
			this.oDropDown.selectedIndex = 0;
			this.oText.value = this.oDropDown.options[this.oDropDown.selectedIndex].text;
			this.oHidden.value = this.oDropDown.options[this.oDropDown.selectedIndex].value;
		}
	}
	else
	{
		this.oHidden.value = "";
		this.query = this.oText.value;
		this.GetData();
	}
}

SmartCombo.prototype.GetData = function()
{
	while ( this.oDropDown.hasChildNodes() )
		this.oDropDown.removeChild(this.oDropDown.firstChild);
		
	var xmlhttp = new XMLHttpRequest();
	xmlhttp.open("GET", this.oUrl + encodeURIComponent(this.query), false);
	// if needed set header information 
	// using the setRequestHeader method
	xmlhttp.send(null);
	this.xmlDoc = xmlhttp.responseXML;

	if(document.all)
	{
		this.xmlDoc.setProperty("SelectionLanguage", "XPath");
	}

	var objNodeList = this.xmlDoc.selectNodes("//R");
	
	for(i=0;i<objNodeList.length;i++)
	{
		var oOption = document.createElement('option');
		this.oDropDown.appendChild(oOption);
		oOption.text = Sarissa.getText(objNodeList[i].selectSingleNode("T"), true);
		oOption.value = Sarissa.getText(objNodeList[i].selectSingleNode("V"), true);
	}
	this.oDiv.style.visibility = "visible";
	this.oDropDown.style.visibility = "visible";
	
	if(!this.oDropDown.hasChildNodes())
	{
		this.oDiv.style.visibility = "hidden";
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
