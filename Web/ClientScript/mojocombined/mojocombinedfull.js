//misc functions
function watermarkEnter(obj,wm){if(obj.value==wm){obj.value='';obj.style.color='';obj.select();}}
function watermarkLeave(obj,wm){if(obj.value==''){obj.value=wm;}}
function trackUserLocation(location){if(location==null){return;}}
function Get_Cookie(check_name){var a_all_cookies=document.cookie.split(';');var a_temp_cookie='';var cookie_name='';var cookie_value='';var b_cookie_found=false;var i='';for(i=0;i<a_all_cookies.length;i++)
{a_temp_cookie=a_all_cookies[i].split('=');cookie_name=a_temp_cookie[0].replace(/^\s+|\s+$/g,'');if(cookie_name==check_name)
{b_cookie_found=true;if(a_temp_cookie.length>1)
{cookie_value=unescape(a_temp_cookie[1].replace(/^\s+|\s+$/g,''));}
return cookie_value;break;}
a_temp_cookie=null;cookie_name='';}
if(!b_cookie_found)
{return null;}}
function Set_Cookie(name,value,expires,path,domain,secure){var today=new Date();today.setTime(today.getTime());if(expires)
{expires=expires*1000*60*60*24;}
var expires_date=new Date(today.getTime()+(expires));document.cookie=name+"="+escape(value)+
((expires)?";expires="+expires_date.toGMTString():"")+
((path)?";path="+path:"")+
((domain)?";domain="+domain:"")+
((secure)?";secure":"");}
function Delete_Cookie(name,path,domain){if(Get_Cookie(name))document.cookie=name+"="+
((path)?";path="+path:"")+
((domain)?";domain="+domain:"")+";expires=Thu, 01-Jan-1970 00:00:01 GMT";}
/* page exit prompt */
var exitThisPagePrompt = 'Are you sure you want to leave this page?'; var alreadySetExitPrompt = false; var requireExitPrompt = false; var contentDidChange = false;
function goodbye(e) { if (!requireExitPrompt) { return; } if (!contentDidChange) { return; } return exitThisPagePrompt; }
function hookupGoodbyePrompt(message) { contentDidChange = true; if ((!alreadySetExitPrompt)&&(requireExitPrompt)) { exitThisPagePrompt = message; alreadySetExitPrompt = true; window.onbeforeunload = goodbye;} }
function unHookGoodbyePrompt() { requireExitPrompt = false; }
