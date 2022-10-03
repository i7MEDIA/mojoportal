<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ThreadList.ascx.cs" Inherits="mojoPortal.Web.ForumUI.ThreadList" %>
<%@ Register Namespace="mojoPortal.Web.ForumUI" Assembly="mojoPortal.Features.UI" TagPrefix="forum" %>

<forum:ForumDisplaySettings ID="displaySettings" runat="server" />

<asp:Panel ID="pnlNotify" runat="server" Visible="false" CssClass="forumnotify">
	<asp:HyperLink ID="lnkNotify" runat="server" CssClass="fsubcribe1 fsubscribe1" SkinID="ForumSubscribe1" />
	<asp:HyperLink ID="lnkNotify2" runat="server" CssClass="fsubcribe2 fsubscribe2" SkinID="ForumSubscribe2" />
</asp:Panel>

<div class="modulepager">
	<portal:mojoCutePager ID="pgrTop" runat="server" />
	<asp:HyperLink runat="server" ID="lnkNewThread" CssClass="ModulePager newthread" EnableViewState="false" SkinID="ForumNewThreadButtonTop" />
	<asp:HyperLink ID="lnkLogin" runat="server" CssClass="ModulePager" SkinID="ForumLoginLink" />
</div>

<table summary='<%# Resources.ForumResources.ForumViewTableSummary %>' class='<%= displaySettings.ThreadListCssClass %>' <% if (displaySettings.UseOldTableAttributes) {%> style="width: 100%;" <% } %>>
	<thead>
		<tr class="moduletitle">
			<th id='t1' class="ftitle"><mp:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="ForumViewSubjectLabel" ResourceFile="ForumResources" UseLabelTag="false" /></th>
			<th id='t2' class="fstartedby"><mp:SiteLabel ID="lblForumStartedBy" runat="server" ConfigKey="ForumViewStartedByLabel" ResourceFile="ForumResources" UseLabelTag="false" /></th>
			<th id='t3' class="fpostviews"><mp:SiteLabel ID="lblTotalViewsCountLabel" runat="server" ConfigKey="ForumViewViewCountLabel" ResourceFile="ForumResources" UseLabelTag="false" /></th>
			<th id='t4' class="fpostreplies"><mp:SiteLabel ID="lblTotalRepliesCountLabel" runat="server" ConfigKey="ForumViewReplyCountLabel" ResourceFile="ForumResources" UseLabelTag="false" /></th>
			<th id='t5' class="fpostdate"><mp:SiteLabel ID="lblLastPostLabel" runat="server" ConfigKey="ForumViewPostLastPostLabel" ResourceFile="ForumResources" UseLabelTag="false" /></th>
		</tr>
	</thead>
	<asp:Repeater ID="rptForums" runat="server">
		<HeaderTemplate><tbody></HeaderTemplate>
		<ItemTemplate>
			<tr class='modulerow <%# GetRowCssClass(Convert.ToInt32(Eval("SortOrder")),Convert.ToBoolean(Eval("IsLocked"))) %>'>
				<td headers='t1' class="ftitle">
					<span class='<%# GetFolderCssClass(Convert.ToInt32(Eval("SortOrder")),Convert.ToBoolean(Eval("IsLocked"))) %>'></span>
					
					<asp:HyperLink ID="editLink"
						CssClass="threadEdit"
						ToolTip="<%# Resources.ForumResources.ForumThreadEditLabel %>"
						NavigateUrl='<%# SiteRoot + "/Forums/EditThread.aspx?thread=" + DataBinder.Eval(Container.DataItem,"ThreadID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString()  %>'
						Visible='<%# GetPermission(DataBinder.Eval(Container.DataItem,"StartedByUserID"))%>' runat="server" />
					<portal:NoFollowHyperlink runat="server"
						ID="HyperLink3"
						ToolTip="RSS" CssClass="forumfeed"
						NavigateUrl='<%# NonSslSiteRoot + "/Forums/RSS.aspx?pageid=" + PageId.ToString() + "&m=" + ModuleId.ToString() + "~" + ItemId.ToString()  + "~" + Eval("ThreadID") %>'
						Visible="<%# Config.EnableRSSAtThreadLevel %>" />
					<a href='<%# FormatUrl(Convert.ToInt32(Eval("ThreadID"))) %>'>
						<%# Server.HtmlEncode(Eval("ThreadSubject").ToString())%>
					</a>
				</td>
				<td headers='t2' class="fstartedby">
					<%# Eval("StartedBy") %>
				</td>
				<td headers='t3' class="fpostviews">
					<%# Eval("TotalViews") %>
				</td>
				<td headers='t4' class="fpostreplies">
					<%# Eval("TotalReplies") %>
				</td>
				<td headers='t5' class="fpostdate">
					<%# FormatDate(Convert.ToDateTime(Eval("MostRecentPostDate"))) %>
					<br />
					<%# Eval("MostRecentPostUser") %>
				</td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class='modulealtrow <%# GetRowCssClass(Convert.ToInt32(Eval("SortOrder")),Convert.ToBoolean(Eval("IsLocked"))) %>'>
				<td headers='t1' class="ftitle">
					<span class='<%# GetFolderCssClass(Convert.ToInt32(Eval("SortOrder")),Convert.ToBoolean(Eval("IsLocked"))) %>'></span>
					<asp:HyperLink runat="server"
						ID="editLink"
						CssClass="threadEdit"
						ToolTip="<%# Resources.ForumResources.ForumThreadEditLabel %>"
						NavigateUrl='<%# SiteRoot + "/Forums/EditThread.aspx?thread=" + DataBinder.Eval(Container.DataItem,"ThreadID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString()  %>'
						Visible='<%# GetPermission(DataBinder.Eval(Container.DataItem,"StartedByUserID"))%>' />
					<portal:NoFollowHyperlink runat="server"
						ID="HyperLink3"
						ToolTip="RSS"
						CssClass="forumfeed"
						NavigateUrl='<%# NonSslSiteRoot + "/Forums/RSS.aspx?pageid=" + PageId.ToString() + "&m=" + ModuleId.ToString() + "~" + ItemId.ToString()  + "~" + Eval("ThreadID") %>'
						Visible="<%# Config.EnableRSSAtThreadLevel %>" />
					<a href='<%# FormatUrl(Convert.ToInt32(Eval("ThreadID"))) %>'>
						<%# Server.HtmlEncode(Eval("ThreadSubject").ToString())%>
					</a>
				</td>
				<td headers='t2' class="fstartedby"><%# Eval("StartedBy") %></td>
				<td headers='t3' class="fpostviews"><%# Eval("TotalViews") %></td>
				<td headers='t4' class="fpostreplies"><%# Eval("TotalReplies") %></td>
				<td headers='t5' class="fpostdate">
					<%# FormatDate(Convert.ToDateTime(Eval("MostRecentPostDate"))) %>
					<br />
					<%# Eval("MostRecentPostUser") %>
				</td>
			</tr>
		</AlternatingItemTemplate>
		<FooterTemplate></tbody></FooterTemplate>
	</asp:Repeater>
</table>

<div class="modulepager">
	<asp:HyperLink runat="server" ID="lnkNewThreadBottom" CssClass="ModulePager newthread" EnableViewState="false" SkinID="ForumNewThreadButtonBottom" />
	<portal:mojoCutePager ID="pgrBottom" runat="server" EnableViewState="false" />
</div>