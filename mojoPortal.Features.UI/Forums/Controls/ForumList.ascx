<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ForumList.ascx.cs" Inherits="mojoPortal.Web.ForumUI.ForumList" %>

<asp:Panel ID="pnlForumList" runat="server">
<table summary='<%# Resources.ForumResources.ForumsTableSummary %>'  cellpadding="0" cellspacing="1" border="0" width="100%">
	<thead><tr class="moduletitle">
		<th id="tdSubscribedHead" runat="server" enableviewstate="false" class="fsubscribe">
			<mp:SiteLabel id="lblSubscribed" runat="server" ConfigKey="ForumModuleSubscribedLabel" ResourceFile="ForumResources" UseLabelTag="false" />
		</th>
		<th id='t2' class="ftitle">
			<mp:SiteLabel id="lblForumName" runat="server" ConfigKey="ForumModuleForumLabel" ResourceFile="ForumResources" UseLabelTag="false" />
		</th>
		<th id='t3' class="fthreadcount">
			<mp:SiteLabel id="lblThreadCount" runat="server" ConfigKey="ForumModuleThreadCountLabel" ResourceFile="ForumResources" UseLabelTag="false" />
		</th>
		<th id='t4' class="fpostcount">
			<mp:SiteLabel id="lblPostCount" runat="server" ConfigKey="ForumModulePostCountLabel" ResourceFile="ForumResources" UseLabelTag="false" />
		</th>
		<th id='t5' class="fpostdate">
			<mp:SiteLabel id="lblLastPost" runat="server" ConfigKey="ForumModulePostLastPostLabel" ResourceFile="ForumResources" UseLabelTag="false" />
		</th>
	</tr></thead>
   <asp:Repeater id="rptForums" runat="server" >
      <HeaderTemplate><tbody></HeaderTemplate>
      <ItemTemplate >
         <tr class="modulerow">
            <td headers='<%# tdSubscribedHead.ClientID %>' id="tdSubscribed" runat="server" enableviewstate='<%# ShowSubscribeCheckboxes %>' class="txtmed padded fsubscribe" Visible='<%# Request.IsAuthenticated %>' > 
                <div id="divSbubcriberCount" runat="server" enableviewstate="false" visible='<%# (Config.ShowSubscriberCount &&(!IsEditable)) %>'>
                   <asp:Literal ID="litSubCount" runat="server" EnableViewState="false" Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
                </div> 
                <div id="divEditor" runat="server" visible='<%# IsEditable %>' enableviewstate="false">
                    <asp:HyperLink ID="lnkSubscribers" runat="server" EnableViewState="false" CssClass="cblink"
                    NavigateUrl='<%# SiteRoot + "/Forums/SubscriberDialog.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
                    Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' 
                    ToolTip='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
                </div>
                <div class="forumnotify">
				<asp:HyperLink ID="lnkNotify" runat="server" EnableViewState="false" Visible='<%# !ShowSubscribeCheckboxes %>' ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/FeatureIcons/email.png"  %>' NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>' 
				 Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>' />
                 &nbsp;<asp:HyperLink ID="lnkNotify2" runat="server" EnableViewState="false" Visible='<%# !ShowSubscribeCheckboxes %>' NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>' 
				 Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>'
                 ToolTip='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>' />

                 <a id='forum<%# Eval("ItemID") %>' />
					<asp:CheckBox id="chkSubscribed" runat="server" Visible='<%# ShowSubscribeCheckboxes %>'
						Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Subscribed")) %>'
						OnCheckedChanged="Subscribed_CheckedChanged" EnableViewState="true" />

                 </div>
            </td>
            <td headers='t2' class="ftitle"> 
				<h3><asp:HyperLink id="editLink" runat="server" EnableViewState="false"
				    CssClass="forumEdit"
					Tooltip="<%# Resources.ForumResources.ForumEditForumLabel %>"
				    NavigateUrl='<%# SiteRoot + "/Forums/EditForum.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
				    Visible="<%# IsEditable %>"  />
				<portal:NoFollowHyperlink id="HyperLink3" runat="server" EnableViewState="false"
				    Tooltip="RSS" CssClass="forumfeed" 
				    NavigateUrl='<%# NonSslSiteRoot + "/Forums/RSS.aspx?pageid=" + PageId.ToInvariantString() + "&m=" + ModuleId.ToInvariantString()  +"~" + Eval("ItemID")  %>' 
				    Visible="<%# Config.EnableRSSAtForumLevel %>"  />
				<asp:HyperLink id="viewlink1" runat="server" SkinID="TitleLink" EnableViewState="false"
				    NavigateUrl='<%# FormatUrl(Convert.ToInt32(Eval("ItemID"))) %>'>
				    <%# DataBinder.Eval(Container.DataItem,"Title") %></asp:HyperLink></h3>
				<%# DataBinder.Eval(Container.DataItem,"Description").ToString() %>
            </td>
            <td headers='t3' class="fthreadcount">  
				<%# DataBinder.Eval(Container.DataItem,"ThreadCount") %>
            </td>
            <td headers='t4' class="fpostcount">  
				<%# DataBinder.Eval(Container.DataItem,"PostCount") %>
            </td>
            <td headers='t5' class="fpostdate">  
                <%# FormatDate(Eval("MostRecentPostDate")) %>
            </td>
         </tr>
      </ItemTemplate>
      <alternatingItemTemplate>
		<tr class="modulealtrow">
            <td headers='<%# tdSubscribedHead.ClientID %>' id="tdSubscribedAlt" runat="server" enableviewstate='<%# ShowSubscribeCheckboxes %>' class="txtmed padded fsubscribe" Visible='<%# Request.IsAuthenticated %>'>  
                <div id="divSbubcriberCount" runat="server" enableviewstate="false" visible='<%# (Config.ShowSubscriberCount &&(!IsEditable)) %>'>
                   <asp:Literal ID="litSubCount" runat="server" EnableViewState="false" Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
                </div> 
                <div id="divEditor" runat="server" enableviewstate="false" visible='<%# IsEditable %>'>
                    <asp:HyperLink ID="lnkSubscribers" runat="server" EnableViewState="false" CssClass="cblink" 
                    NavigateUrl='<%# SiteRoot + "/Forums/SubscriberDialog.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
                    Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' 
                    ToolTip='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
                </div>
                <div class="forumnotify">
                <asp:HyperLink ID="lnkNotify" runat="server" EnableViewState="false" Visible='<%# !ShowSubscribeCheckboxes %>' ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/FeatureIcons/email.png"  %>' NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>' 
				 Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>' />
                 &nbsp;<asp:HyperLink ID="lnkNotify2" runat="server" EnableViewState="false" Visible='<%# !ShowSubscribeCheckboxes %>' NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>' 
				 Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>'
                 ToolTip='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>' />
                 <a id='forum<%# Eval("ItemID") %>' />
					<asp:CheckBox id="chkSubscribedAlt" runat="server" Visible='<%# ShowSubscribeCheckboxes %>'
						Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Subscribed")) %>'
						OnCheckedChanged="Subscribed_CheckedChanged" EnableViewState="true"  />
                 </div>
            </td>
            <td headers='t2' class="ftitle"> 
				<h3><asp:HyperLink id="Hyperlink1" runat="server" EnableViewState="false"
				    CssClass="forumEdit"
					Tooltip="<%# Resources.ForumResources.ForumEditForumLabel %>" 
				    NavigateUrl='<%# SiteRoot + "/Forums/EditForum.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
				    Visible="<%# IsEditable %>"  />
				<portal:NoFollowHyperlink id="HyperLink3" runat="server" EnableViewState="false"
				    Tooltip="RSS" CssClass="forumfeed" 
				    NavigateUrl='<%# NonSslSiteRoot + "/Forums/RSS.aspx?pageid=" + PageId.ToInvariantString() + "&m=" + ModuleId.ToInvariantString()  +"~" + Eval("ItemID")  %>' 
				    Visible="<%# Config.EnableRSSAtForumLevel %>"  />
				<asp:HyperLink id="Hyperlink2" runat="server" SkinID="TitleLink" EnableViewState="false"
				    NavigateUrl='<%# FormatUrl(Convert.ToInt32(Eval("ItemID"))) %>' >
				    <%# DataBinder.Eval(Container.DataItem,"Title") %></asp:HyperLink></h3>
				<%# DataBinder.Eval(Container.DataItem,"Description").ToString()%>
            </td>
            <td headers='t3' class="fthreadcount">  
				<%# DataBinder.Eval(Container.DataItem,"ThreadCount") %>
            </td>
            <td headers='t4' class="fpostcount">  
				<%# DataBinder.Eval(Container.DataItem,"PostCount") %>
            </td>
            <td headers='t5' class="fpostdate">  
                 <%# FormatDate(Eval("MostRecentPostDate")) %>
            </td>
         </tr>
      </AlternatingItemTemplate>
      <FooterTemplate></tbody></FooterTemplate>
   </asp:Repeater>
   <tr id="trSubscribeButtons" runat="server">
			<td id="tdSave" runat="server" class="settingrow forum" align="left" colspan="5">
				<portal:mojoButton id="btnSave" runat="server" Text="Save" />
				<portal:mojoButton id="btnCancel" runat="server" Text="Cancel" />
				<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="forumeditsubscriptionshelp" />
			</td>
		</tr>
</table>
<div id="divEditSubscriptions" runat="server" enableviewstate="false" class="settingrow forumnotification">
    <portal:NoFollowHyperlink ID="lnkModuleRSS" runat="server" EnableViewState="false" CssClass="forumfeed forummodulefeed" />
    <asp:HyperLink id="editSubscriptionsLink" EnableViewState="false" runat="server" CssClass="editforumsubcriptions" />
</div>
</asp:Panel>
