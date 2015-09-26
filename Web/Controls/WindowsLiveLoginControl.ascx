<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="WindowsLiveLoginControl.ascx.cs" Inherits="mojoPortal.Web.UI.WindowsLiveLoginControl" %>
<div style="background:<%= BackColor %>;color:<%=ForeColor %>; font-size: 14px; width:250px; padding: 3px 3px 3px 3px; border: dashed thin gray;">
<iframe 
       id="WebAuthControl" 
       name="WebAuthControl"
       src="<%= Protocol %>login.live.com/controls/WebAuth.htm?appid=<%=WindowsLiveAppId%>&amp;style=font-size%3A+small%3B+font-family%3A+verdana%3B+background%3A+<%= BackColor %>%3B"
       width="97px"
       height="24px"
       marginwidth="0"
       marginheight="0"
       frameborder="0"
       scrolling="no" style="display:inline; padding: 0px 0px 0px 10px; white-space:nowrap;">
   </iframe><span style="vertical-align:top;"><br />&nbsp;&nbsp;<asp:Literal ID="litSignInAddendum" runat="server" /></span>
   </div>