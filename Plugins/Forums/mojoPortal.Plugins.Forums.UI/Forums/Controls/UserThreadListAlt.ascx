<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="UserThreadListAlt.ascx.cs" Inherits="mojoPortal.Web.ForumUI.UserThreadListAlt" %>
<%@ Register Namespace="mojoPortal.Web.ForumUI" Assembly="mojoPortal.Features.UI" TagPrefix="forum" %>

<forum:ForumDisplaySettings ID="displaySettings" runat="server" />

<div class="modulepager"><portal:mojoCutePager ID="pgrTop" runat="server" /></div>
	
<asp:Repeater id="rptForums" runat="server" >
	<HeaderTemplate><ul class='simplelist  <%= displaySettings.UserThreadListCssClass %>'></HeaderTemplate>
	<ItemTemplate>
		<li class='forumwrap threadwrap'>
			<div class="threadtitle"> 
				<img alt="" src='<%# ImageSiteRoot + "/Data/SiteImages/folder.png"  %>'  />
				<a href='<%# FormatThreadUrl(Convert.ToInt32(Eval("ThreadID")),Convert.ToInt32(Eval("ModuleID")),Convert.ToInt32(Eval("ForumID")),Convert.ToInt32(Eval("PageID"))) %>'>
					<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "ThreadSubject").ToString())%></a>
			</div>
			<div class="threadstartedby">  
                <mp:SiteLabel id="lblForumStartedBy" runat="server" ConfigKey="ForumViewStartedByLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
				<%# DataBinder.Eval(Container.DataItem, "StartedBy")%>
			</div>
		</li>
	</ItemTemplate>
	<alternatingItemTemplate>
		<li class='forumwrap threadwrap threadwrapalt'>
			<div class="threadtitle"> 
				<img alt="" src='<%# ImageSiteRoot + "/Data/SiteImages/folder.png"  %>'  />
				<a href='<%# FormatThreadUrl(Convert.ToInt32(Eval("ThreadID")),Convert.ToInt32(Eval("ModuleID")),Convert.ToInt32(Eval("ForumID")),Convert.ToInt32(Eval("PageID"))) %>'>
					<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "ThreadSubject").ToString())%></a>
			</div>
			<div class="threadstartedby">  
                <mp:SiteLabel id="SiteLabel1" runat="server" ConfigKey="ForumViewStartedByLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
				<%# DataBinder.Eval(Container.DataItem, "StartedBy")%>
			</div>
		</li>
	</AlternatingItemTemplate>
	<FooterTemplate></ul></FooterTemplate>
</asp:Repeater>
	
	<div class="modulepager">
		<portal:mojoCutePager ID="pgrBottom" runat="server" />
	</div>
