
function DateRangeHelper(txtBeginDate, txtEndDate, daysToAdd, formatString)
{
    if(txtBeginDate == undefined)return;
    if(txtEndDate == undefined)return;
    
    this.txtBeginDate = txtBeginDate;
    this.txtEndDate = txtEndDate;
    this.daysToAdd = daysToAdd;
    this.formatString = formatString;
    this.txtBeginDate.DateRangeHelper = this;
    //this.outputText.DateRangeHelper = this;
    
    this.txtBeginDate.onblur = DateRangeHelper.prototype.onblur;
    
    //this.outputText.onblur = DateRangeHelper.prototype.onUrlBlur;

}

 DateRangeHelper.prototype.onblur = function()
 {
    //alert('you blurred');
    this.DateRangeHelper.onchange();
 }



DateRangeHelper.prototype.onchange = function()
{
    try
    {
	    var begin = new Date();
	    //begin.setTime(Date.parse(this.txtBeginDate.value));
	    //var begin = getDateFromFormat(this.txtBeginDate.value, this.formatString);
	    begin.setTime(getDateFromFormat(this.txtBeginDate.value, this.formatString));
	    //alert(begin);
	    if(this.daysToAdd > 0)
	    {
	        var endDate = addDays(begin, this.daysToAdd);
	        this.txtEndDate.value = formatDate(endDate, this.formatString);
	    }
	    else
	    {
	        this.txtEndDate.value = this.txtBeginDate.value;
	    }
	}
	catch(Error)
	{
	    //alert(Error);
	}
}

function addDays(startDate,daysToAdd) {
    return new Date(startDate.getTime() + daysToAdd*24*60*60*1000);
}

function formatDateold(theDate, formatString)
{
    return formatString.replace('MM', theDate.getMonth() + 1).replace('M', theDate.getMonth() + 1).replace('dd', theDate.getDate()).replace('d', theDate.getDate()).replace('yyyy', theDate.getFullYear());
}

