<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ForumList.ascx.cs" Inherits="mojoPortal.Web.ForumUI.ForumList" %>
<%@ Register Namespace="mojoPortal.Web.ForumUI" Assembly="mojoPortal.Features.UI" TagPrefix="forum" %>

<forum:ForumDisplaySettings ID="displaySettings" runat="server" />
<asp:Panel ID="pnlForumList" runat="server">
	<table summary='<%# Resources.ForumResources.ForumsTableSummary %>' class='<%= displaySettings.ForumListCssClass %>' <% if (displaySettings.UseOldTableAttributes) {%> border="0" style="width: 100%;" <% } %>>
		<thead>
			<tr class="moduletitle">
				<th id="tdSubscribedHead" runat="server" enableviewstate="false" class="fsubscribe">
					<mp:SiteLabel ID="lblSubscribed" runat="server" ConfigKey="ForumModuleSubscribedLabel" ResourceFile="ForumResources" UseLabelTag="false" />
				</th>
				<th id='t2' class="ftitle">
					<mp:SiteLabel ID="lblForumName" runat="server" ConfigKey="ForumModuleForumLabel" ResourceFile="ForumResources" UseLabelTag="false" />
				</th>
				<th id='t3' class="fthreadcount">
					<mp:SiteLabel ID="lblThreadCount" runat="server" ConfigKey="ForumModuleThreadCountLabel" ResourceFile="ForumResources" UseLabelTag="false" />
				</th>
				<th id='t4' class="fpostcount">
					<mp:SiteLabel ID="lblPostCount" runat="server" ConfigKey="ForumModulePostCountLabel" ResourceFile="ForumResources" UseLabelTag="false" />
				</th>
				<th id='t5' class="fpostdate">
					<mp:SiteLabel ID="lblLastPost" runat="server" ConfigKey="ForumModulePostLastPostLabel" ResourceFile="ForumResources" UseLabelTag="false" />
				</th>
			</tr>
		</thead>
		<asp:Repeater ID="rptForums" runat="server">
			<HeaderTemplate>
				<tbody>
			</HeaderTemplate>
			<ItemTemplate>
				<tr class="modulerow">
					<td runat="server"
						headers='<%# tdSubscribedHead.ClientID %>'
						id="tdSubscribed"
						enableviewstate='<%# ShowSubscribeCheckboxes %>'
						class="txtmed padded fsubscribe"
						visible='<%# Request.IsAuthenticated %>'
					>
						<div id="divSbubcriberCount" runat="server" enableviewstate="false" visible='<%# (Config.ShowSubscriberCount &&(!IsEditable)) %>'>
							<asp:Literal ID="litSubCount" runat="server" EnableViewState="false" Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
						</div>
						<div runat="server"
							id="divEditor"
							visible='<%# IsEditable %>'
							enableviewstate="false"
						>
							<asp:HyperLink runat="server"
								ID="lnkSubscribers"
								EnableViewState="false"
								CssClass="cblink"
								NavigateUrl='<%# SiteRoot + "/Forums/SubscriberDialog.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>'
								Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>'
								ToolTip='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>'
							/>
						</div>
						<div class="forumnotify">
							<asp:HyperLink runat="server"
								ID="lnkNotify"
								EnableViewState="false"
								Visible='<%# !ShowSubscribeCheckboxes %>'
								ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/email.png"  %>'
								NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>'
								Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>'
							/>
							<asp:HyperLink runat="server"
								ID="lnkNotify2"
								EnableViewState="false"
								Visible='<%# !ShowSubscribeCheckboxes %>'
								NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>'
								Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>'
								ToolTip='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>'
							/>
							<a id='forum<%# Eval("ItemID") %>'></a>
							<asp:CheckBox runat="server"
								ID="chkSubscribed"
								Visible='<%# ShowSubscribeCheckboxes %>'
								Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Subscribed")) %>'
								OnCheckedChanged="Subscribed_CheckedChanged" EnableViewState="true"
							/>
						</div>
					</td>
					<td headers='t2' class="ftitle">
						<h3>
							<asp:HyperLink runat="server"
								ID="editLink"
								EnableViewState="false"
								CssClass="forumEdit"
								ToolTip="<%# Resources.ForumResources.ForumEditForumLabel %>"
								NavigateUrl='<%# SiteRoot + "/Forums/EditForum.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>'
								Visible="<%# IsEditable %>"
							/>
							<portal:NoFollowHyperlink ID="HyperLink3" runat="server" EnableViewState="false"
								ToolTip="RSS" CssClass="forumfeed"
								NavigateUrl='<%# NonSslSiteRoot + "/Forums/RSS.aspx?pageid=" + PageId.ToInvariantString() + "&m=" + ModuleId.ToInvariantString()  +"~" + Eval("ItemID")  %>'
								Visible="<%# Config.EnableRSSAtForumLevel %>"
							/>
							<asp:HyperLink runat="server"
								ID="viewlink1"
								SkinID="TitleLink"
								EnableViewState="false"
								NavigateUrl='<%# FormatUrl(Convert.ToInt32(Eval("ItemID"))) %>'
							>
								<%# DataBinder.Eval(Container.DataItem,"Title") %>
							</asp:HyperLink>
						</h3>
						<%# DataBinder.Eval(Container.DataItem,"Description").ToString() %>
					</td>
					<td headers='t3' class="fthreadcount"><%# DataBinder.Eval(Container.DataItem,"ThreadCount") %></td>
					<td headers='t4' class="fpostcount"><%# DataBinder.Eval(Container.DataItem,"PostCount") %></td>
					<td headers='t5' class="fpostdate"><%# FormatDate(Eval("MostRecentPostDate")) %></td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="modulealtrow">
					<td runat="server"
						headers='<%# tdSubscribedHead.ClientID %>'
						id="tdSubscribedAlt"
						enableviewstate='<%# ShowSubscribeCheckboxes %>'
						class="txtmed padded fsubscribe"
						visible='<%# Request.IsAuthenticated %>'
					>
						<div runat="server"
							id="divSbubcriberCount"
							enableviewstate="false"
							visible='<%# (Config.ShowSubscriberCount &&(!IsEditable)) %>'
						>
							<asp:Literal ID="litSubCount" runat="server" EnableViewState="false" Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
						</div>
						<div id="divEditor" runat="server" enableviewstate="false" visible='<%# IsEditable %>'>
							<asp:HyperLink runat="server"
								ID="lnkSubscribers"
								EnableViewState="false"
								CssClass="cblink"
								NavigateUrl='<%# SiteRoot + "/Forums/SubscriberDialog.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>'
								Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>'
								ToolTip='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>'
							/>
						</div>
						<div class="forumnotify">
							<asp:HyperLink runat="server"
								ID="lnkNotify"
								EnableViewState="false"
								Visible='<%# !ShowSubscribeCheckboxes %>'
								ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/email.png"  %>'
								NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>'
								Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>'
							/>
							<asp:HyperLink runat="server"
								ID="lnkNotify2"
								EnableViewState="false"
								Visible='<%# !ShowSubscribeCheckboxes %>'
								NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>'
								Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>'
								ToolTip='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>'
							/>
							<a id='forum<%# Eval("ItemID") %>' />
							<asp:CheckBox runat="server"
								ID="chkSubscribedAlt"
								Visible='<%# ShowSubscribeCheckboxes %>'
								Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Subscribed")) %>'
								OnCheckedChanged="Subscribed_CheckedChanged" EnableViewState="true"
							/>
						</div>
					</td>
					<td headers='t2' class="ftitle">
						<h3>
							<asp:HyperLink runat="server"
								ID="Hyperlink1"
								EnableViewState="false"
								CssClass="forumEdit"
								ToolTip="<%# Resources.ForumResources.ForumEditForumLabel %>"
								NavigateUrl='<%# SiteRoot + "/Forums/EditForum.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>'
								Visible="<%# IsEditable %>"
							/>
							<portal:NoFollowHyperlink runat="server"
								ID="HyperLink3"
								EnableViewState="false"
								ToolTip="RSS" CssClass="forumfeed"
								NavigateUrl='<%# NonSslSiteRoot + "/Forums/RSS.aspx?pageid=" + PageId.ToInvariantString() + "&m=" + ModuleId.ToInvariantString()  +"~" + Eval("ItemID")  %>'
								Visible="<%# Config.EnableRSSAtForumLevel %>"
							/>
							<asp:HyperLink runat="server"
								ID="Hyperlink2"
								SkinID="TitleLink"
								EnableViewState="false"
								NavigateUrl='<%# FormatUrl(Convert.ToInt32(Eval("ItemID"))) %>'
							>
								<%# DataBinder.Eval(Container.DataItem,"Title") %>
							</asp:HyperLink>
						</h3>
						<%# DataBinder.Eval(Container.DataItem,"Description").ToString()%>
					</td>
					<td headers='t3' class="fthreadcount"><%# DataBinder.Eval(Container.DataItem,"ThreadCount") %></td>
					<td headers='t4' class="fpostcount"><%# DataBinder.Eval(Container.DataItem,"PostCount") %></td>
					<td headers='t5' class="fpostdate"><%# FormatDate(Eval("MostRecentPostDate")) %></td>
				</tr>
			</AlternatingItemTemplate>
			<FooterTemplate></tbody></FooterTemplate>
		</asp:Repeater>

		<tr id="trSubscribeButtons" runat="server">
			<td id="tdSave" runat="server" class="settingrow forum" align="left" colspan="5">
				<portal:mojoButton ID="btnSave" runat="server" Text="Save" SkinID="SuccessButton" />
				<portal:mojoButton ID="btnCancel" runat="server" Text="Cancel" SkinID="DefaultButton" />
				<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="forumeditsubscriptionshelp" />
			</td>
		</tr>
	</table>

	<div id="divEditSubscriptions" runat="server" enableviewstate="false" class="settingrow forumnotification">
		<portal:NoFollowHyperlink ID="lnkModuleRSS" runat="server" EnableViewState="false" CssClass="forumfeed forummodulefeed" SkinID="ForumNoFollow" />
		<asp:HyperLink ID="editSubscriptionsLink" EnableViewState="false" runat="server" CssClass="editforumsubcriptions" SkinID="EditForumSubscriptionsLink" />
	</div>
</asp:Panel>
