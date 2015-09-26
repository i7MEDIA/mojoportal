
function checkProtocol(spanStatus) {
  if (location.protocol.indexOf('http') != 0) {
    setError(spanStatus,'This sample must be hosted on an HTTP server');
    return false;
  } else {
    return true;
  }
}

function addStatus(spanStatus, statusMessage, opt_class) {
  
  if (!spanStatus) return;
  var node = document.createTextNode(statusMessage);
  if (opt_class) {
    var span = document.createElement('span');
    span.className = opt_class;
    span.appendChild(node);
    node = span;
  }
  spanStatus.appendChild(node);
  spanStatus.appendChild(document.createElement('br'));
}

function clearStatus(spanStatus) {
  while (spanStatus && spanStatus.firstChild) {
    spanStatus.removeChild(spanStatus.firstChild);
  }
}

function setError(spanStatus, errorMessage) {
  clearStatus(spanStatus);
  addStatus(spanStatus, errorMessage, 'error');
}


function escapeHtml(s) {
  return String(s).replace(/&/g, '&amp;').replace(/</g, '&lt;');
}

function isCursorOnFirstLine(ta) {
  if (typeof ta.selectionStart == 'number') {
    var index = ta.value.indexOf('\n');
    return index == -1 || ta.selectionStart <= index;
  } else {
    // Get the range representing the text before the cursor. Then get the
    // number of rects that is and see if we have more than one
    var selectionRange = document.selection.createRange();
    var range = selectionRange.duplicate();
    range.moveToElementText(ta);
    range.setEndPoint('EndToStart', selectionRange);
    return range.getClientRects().length == 1;
  }
}


function isCursorOnLastLine(ta) {
  if (typeof ta.selectionEnd == 'number') {
    var index = ta.value.substr(ta.selectionEnd).indexOf('\n');
    return index == -1;
  } else {
    // Get the range representing the text before the cursor. Then get the
    // number of rects that is and see if we have more than one
    var selectionRange = document.selection.createRange();
    var range = selectionRange.duplicate();
    range.moveToElementText(ta);
    range.setEndPoint('StartToEnd', selectionRange);
    return range.getClientRects().length == 1;
  }
}

function updateTextAreaRows(ta) {
  ta.rows = ta.value.split(/\n/).length + 2;
}


function printRecordset(rs, outputDiv, successNoDataMessage) 
{
  var error = false, errorMessage;
  var i, cols, sb, row;
  sb = [];
  
  if (!rs || error) 
  {
    sb.push("<table class='scrolltable' cellspacing='0'><thead><tr>");
    sb.push('<th>Error</th><thead><tbody><tr><td>',
            errorMessage || 'Unknown error',
            '</td></tr>');
    sb.push('</tbody></table>');
  } 
  else  
  {
    // If we did an update, insert, delete etc. we would not have a valid row
    if (!rs.isValidRow())
    {
    
    	sb.push(successNoDataMessage);
    	
    }
    else
    {
        sb.push("<table class='scrolltable' cellspacing='0'><thead><tr>");
    	// headers
    	cols = rs.fieldCount()
    	for (i = 0; i < cols; i++) 
    	{
      	   sb.push('<th>', escapeHtml(rs.fieldName(i)), '</th>');
    
    	}
    	sb.push('</tr></thead><tbody>');

    	var odd = true;
    	while (rs.isValidRow()) 
    	{
      	    sb.push('<tr>');
       	    for (i = 0; i < cols; i++) 
            {
          	sb.push('<td class=' + (odd ? 'odd' : 'even') + '>',
                escapeHtml(rs.field(i)),/* ' [', typeof rs.field(i), ']',*/
                '</td>');
            }
      	    odd = !odd;
      	    sb.push('</tr>');
      	    rs.next();
     	}
     	rs.close();
       sb.push('</tbody></table>');
     }
     
  }
  
  var output = document.createElement('div');
  outputDiv.innerHTML = '';
  outputDiv.appendChild(output);
  output.innerHTML = sb.join('');
}

function populateDropdownList(rs, dropdownList, valueFieldIndex, textFieldIndex) 
{
  if (!rs) 
  {
    return;
  } 
  else
  {
    while (dropdownList.options.length > 0) {
        dropdownList.options[0] = null;
	}
  
    while (rs.isValidRow()) 
    {
     	addOption(dropdownList,escapeHtml(rs.field(textFieldIndex)),escapeHtml(rs.field(valueFieldIndex))); 
     	rs.next();
     }
     
  }
    
  rs.close();
}
  


function addOption(selectbox,text,value )
{
//alert(selectbox);
	var optn = document.createElement("option");
	optn.text = text;
	optn.value = value;
	selectbox.options.add(optn);
}

function scheduleFunction(timerId, timeout, functionName) {
  cancelSchedule(timerId);
  timerId = window.setTimeout(functionName, timeout);
}

function cancelSchedule(timerId) {
  if (timerId) {
    timerId = window.clearTimeout(timerId);
  }
}
