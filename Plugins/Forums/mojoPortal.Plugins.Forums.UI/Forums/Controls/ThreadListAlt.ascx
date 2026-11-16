<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ThreadListAlt.ascx.cs" Inherits="mojoPortal.Web.ForumUI.ThreadListAlt" %>
<%@ Register Namespace="mojoPortal.Web.ForumUI" Assembly="mojoPortal.Features.UI" TagPrefix="forum" %>
<forum:ForumDisplaySettings ID="displaySettings" runat="server" />
<asp:HyperLink ID="lnkLogin" runat="server" CssClass="flogin" />
<div class="modulepager" id="divPagerTop" runat="server">
    <portal:mojoCutePager ID="pgrTop" runat="server" />   
</div>	
<a href="" class="newthread" id="lnkNewThread" runat="server"></a>  		
<asp:Repeater id="rptForums" runat="server" >
    <HeaderTemplate><ul class="simplelist threadlist"></HeaderTemplate>
	<ItemTemplate>
		<li class='forumwrap threadwrap <%# GetRowCssClass(Convert.ToInt32(Eval("SortOrder")),Convert.ToBoolean(Eval("IsLocked"))) %>'>
			<div class="threadtitle">
			    <span class='<%# GetFolderCssClass(Convert.ToInt32(Eval("SortOrder")),Convert.ToBoolean(Eval("IsLocked"))) %>'>&nbsp;</span>
					<asp:HyperLink id="editLink" 
					CssClass="threadEdit"
					Tooltip="<%# Resources.ForumResources.ForumThreadEditLabel %>"
					NavigateUrl='<%# SiteRoot + "/Forums/EditThread.aspx?thread=" + DataBinder.Eval(Container.DataItem,"ThreadID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString()  %>' 
					Visible='<%# GetPermission(DataBinder.Eval(Container.DataItem,"StartedByUserID"))%>' runat="server" />
					<portal:NoFollowHyperlink id="HyperLink3" runat="server"
		            Tooltip="RSS" CssClass="forumfeed"
		            NavigateUrl='<%# NonSslSiteRoot + "/Forums/RSS.aspx?pageid=" + PageId.ToString() + "&m=" + ModuleId.ToString() + "~" + ItemId.ToString()  + "~" + Eval("ThreadID") %>' 
		            Visible="<%# Config.EnableRSSAtThreadLevel && !displaySettings.HideFeedLinks %>"  />
					<a href='<%# FormatUrl(Convert.ToInt32(Eval("ThreadID"))) %>'>
					<%# Server.HtmlEncode(Eval("ThreadSubject").ToString())%></a>		
			</div>
            <div class="forumstats">
                <div class="threadstartedby" id="divStartedBy1" runat="server" visible='<%# !displaySettings.ForumViewHideStartedBy %>'>
                <mp:SiteLabel id="lblForumStartedBy" runat="server" ConfigKey="ForumViewStartedByLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
                <%# Eval("StartedBy") %>
                </div>
                <div class="threadviewcount" id="divViewCount1" runat="server" visible='<%# !displaySettings.ForumViewHideTotalViews %>'>
			    <mp:SiteLabel id="lblTotalViewsCountLabel" runat="server" ConfigKey="ForumViewViewCountLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
				<%# Eval("TotalViews") %>
                </div>
                 <div class="threadreplycount" id="divReplyCount1" runat="server" visible='<%# !displaySettings.ForumViewHideTotalReplies %>'>
			    <mp:SiteLabel id="lblTotalRepliesCountLabel" runat="server" ConfigKey="ForumViewReplyCountLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
				<%# Eval("TotalReplies") %>
                </div>
                <div class="threadlastpost" id="divLastPostDate1" runat="server" visible='<%# !displaySettings.ForumViewHideLastPostDate %>'>
			    <mp:SiteLabel id="lblLastPostLabel" runat="server" ConfigKey="ForumViewPostLastPostLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />	
				<%# FormatDate(Convert.ToDateTime(Eval("MostRecentPostDate"))) %>
                <div class="threadlastuser" id="divLastPostUser1" runat="server" visible='<%# !displaySettings.ForumViewHideLastPostUser %>'>
                <mp:SiteLabel id="SiteLabel5" runat="server" ConfigKey="LastPostBy" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
				<%# Eval("MostRecentPostUser") %>
                </div>
                </div>
            </div>
			
		</li>
	</ItemTemplate>
	<alternatingItemTemplate>
		<li class='forumwrap threadwrap threadwrapalt <%# GetRowCssClass(Convert.ToInt32(Eval("SortOrder")),Convert.ToBoolean(Eval("IsLocked"))) %>'>
			<div class="threadtitle">
			    <span class='<%# GetFolderCssClass(Convert.ToInt32(Eval("SortOrder")),Convert.ToBoolean(Eval("IsLocked"))) %>'>&nbsp;</span>
				<asp:HyperLink id="editLink"
				CssClass="threadEdit"
				Tooltip="<%# Resources.ForumResources.ForumThreadEditLabel %>"
				NavigateUrl='<%# SiteRoot + "/Forums/EditThread.aspx?thread=" + DataBinder.Eval(Container.DataItem,"ThreadID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString()  %>' 
				Visible='<%# GetPermission(DataBinder.Eval(Container.DataItem,"StartedByUserID"))%>' 
				runat="server" />
			    <portal:NoFollowHyperlink id="HyperLink3" runat="server"
		            Tooltip="RSS" CssClass="forumfeed"
		            NavigateUrl='<%# NonSslSiteRoot + "/Forums/RSS.aspx?pageid=" + PageId.ToString() + "&m=" + ModuleId.ToString() + "~" + ItemId.ToString()  + "~" + Eval("ThreadID") %>' 
		            Visible="<%# Config.EnableRSSAtThreadLevel && !displaySettings.HideFeedLinks %>"  />
				<a href='<%# FormatUrl(Convert.ToInt32(Eval("ThreadID"))) %>'>
					<%# Server.HtmlEncode(Eval("ThreadSubject").ToString())%></a>
            </div>
			 <div class="forumstats">
                <div class="threadstartedby" id="divStartedBy2" runat="server" visible='<%# !displaySettings.ForumViewHideStartedBy %>'>
                <mp:SiteLabel id="SiteLabel1" runat="server" ConfigKey="ForumViewStartedByLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
				<%# Eval("StartedBy") %>
                </div>
                 <div class="threadviewcount" id="divViewCount2" runat="server" visible='<%# !displaySettings.ForumViewHideTotalViews %>'>
			    <mp:SiteLabel id="SiteLabel2" runat="server" ConfigKey="ForumViewViewCountLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
				<%# Eval("TotalViews") %>
                </div>
                <div class="threadreplycount" id="divReplyCount2" runat="server" visible='<%# !displaySettings.ForumViewHideTotalReplies %>'>
			    <mp:SiteLabel id="SiteLabel3" runat="server" ConfigKey="ForumViewReplyCountLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
				<%# Eval("TotalReplies") %>
			    </div>
                <div class="threadlastpost" id="divLastPostDate2" runat="server" visible='<%# !displaySettings.ForumViewHideLastPostDate %>'>
                <mp:SiteLabel id="SiteLabel4" runat="server" ConfigKey="ForumViewPostLastPostLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />	
				<%# FormatDate(Convert.ToDateTime(Eval("MostRecentPostDate"))) %>
                <div class="threadlastuser" id="divLastPostUser1" runat="server" visible='<%# !displaySettings.ForumViewHideLastPostUser %>'>
                <mp:SiteLabel id="SiteLabel6" runat="server" ConfigKey="LastPostBy" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
				<%# Eval("MostRecentPostUser") %>
                </div>
                </div>

			</div>
		</li>
	</AlternatingItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:Repeater>
	
    <div class="modulepager" id="divPagerBottom" runat="server">
		<portal:mojoCutePager ID="pgrBottom" runat="server" EnableViewState="false" />
    </div>
    <a href="" class="newthread" id="lnkNewThreadBottom" runat="server" EnableViewState="false"></a>
    <asp:Panel ID="pnlNotify" runat="server" Visible="false" CssClass="forumnotify">
        <asp:HyperLink ID="lnkNotify" runat="server" CssClass="fsubcribe1 fsubscribe1"  />
        <asp:HyperLink ID="lnkNotify2" runat="server" CssClass="fsubcribe2 fsubscribe2"  />
    </asp:Panel>
