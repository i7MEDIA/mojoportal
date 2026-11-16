<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ForumListAlt.ascx.cs" Inherits="mojoPortal.Web.ForumUI.ForumListAlt" %>
<%@ Register Namespace="mojoPortal.Web.ForumUI" Assembly="mojoPortal.Features.UI" TagPrefix="forum" %>
<forum:ForumDisplaySettings ID="displaySettings" runat="server" />
<asp:panel id="pnlForumList" runat="server" CssClass="forumlist">
    <asp:repeater id="rptForums" runat="server">
        <HeaderTemplate><ul class='simplelist <%= displaySettings.ForumListCssClass %>'></HeaderTemplate>
        <itemtemplate>
         <li class="forumwrap">
            <div class="ftitle"> 
				<h3><asp:HyperLink id="editLink" runat="server" EnableViewState="false"
				    CssClass="forumEdit"
					Tooltip="<%# Resources.ForumResources.ForumEditForumLabel %>"
				    NavigateUrl='<%# SiteRoot + "/Forums/EditForum.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
				    Visible="<%# IsEditable %>"  />
				<portal:NoFollowHyperlink id="HyperLink3" runat="server" EnableViewState="false"
				    Tooltip="RSS" CssClass="forumfeed" 
				    NavigateUrl='<%# NonSslSiteRoot + "/Forums/RSS.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
				    Visible="<%# Config.EnableRSSAtForumLevel && !displaySettings.HideFeedLinks %>"  />
				<asp:HyperLink id="viewlink1" runat="server" SkinID="TitleLink" EnableViewState="false"
				    NavigateUrl='<%# FormatUrl(Convert.ToInt32(Eval("ItemID"))) %>'>
				    <%# DataBinder.Eval(Container.DataItem,"Title") %></asp:HyperLink></h3>	
            </div>
            <div class="fdesc">
            <%# DataBinder.Eval(Container.DataItem,"Description").ToString()%>
            </div>
            <div class="forumstats">
                <div class="forumthreadcount">
                <mp:SiteLabel id="lblThreadCount" runat="server" ConfigKey="ForumModuleThreadCountLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
				<%# DataBinder.Eval(Container.DataItem,"ThreadCount") %>
                </div>
                <div class="forumpostcount">
                <mp:SiteLabel id="lblPostCount" runat="server" ConfigKey="ForumModulePostCountLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
				<%# DataBinder.Eval(Container.DataItem,"PostCount") %>
                </div>
                <div class="forumlastpost">
                <mp:SiteLabel id="lblLastPost" runat="server" ConfigKey="ForumModulePostLastPostLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
                <%# FormatDate(Eval("MostRecentPostDate")) %>
                </div>
            </div>
            <div  id="divSubscribed" runat="server" enableviewstate='<%# ShowSubscribeCheckboxes %>' class="fsubscribe" Visible='<%# Request.IsAuthenticated %>' > 
                <div id="div1" runat="server" enableviewstate="false" visible='<%# (Config.ShowSubscriberCount &&(!IsEditable)) %>' class="forumsubstats">
                   <asp:Literal ID="litSubCount" EnableViewState="false" runat="server" Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
                </div> 
                <div id="div2" runat="server" enableviewstate="false" visible='<%# IsEditable %>' class="forumsubstats">
                    <asp:HyperLink ID="lnkSubscribers" runat="server" EnableViewState="false" CssClass="cblink fsubscribers"
                    NavigateUrl='<%# SiteRoot + "/Forums/SubscriberDialog.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
                    Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' 
                    ToolTip='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
                </div>
                <div class="forumnotify">
				<asp:HyperLink ID="lnkNotify" runat="server" EnableViewState="false" CssClass="fsubcribe1 fsubscribe1" Visible='<%# !ShowSubscribeCheckboxes %>' ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/email.png"  %>' NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>' 
				 Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>' />
                 <asp:HyperLink ID="lnkNotify2" runat="server" CssClass="fsubcribe2 fsubscribe2" Visible='<%# !ShowSubscribeCheckboxes %>' NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>' 
				 Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>'
                 ToolTip='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>' />

                 <a id='forum<%# Eval("ItemID") %>' />
					<asp:CheckBox id="chkSubscribed" runat="server" Visible='<%# ShowSubscribeCheckboxes %>' Text="<%$ Resources:ForumResources, ForumModuleSubscribedLabel %>"
						Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Subscribed")) %>'
						OnCheckedChanged="Subscribed_CheckedChanged" EnableViewState="true" />
                 </div>
          </div>
       </li>
      </itemtemplate>
      <alternatingitemtemplate>
		<li class="forumwrap forumwrapalt">
            <div class="ftitle"> 
				<h3><asp:HyperLink id="Hyperlink1" runat="server" EnableViewState="false"
				    CssClass="forumEdit"
					Tooltip="<%# Resources.ForumResources.ForumEditForumLabel %>" 
				    NavigateUrl='<%# SiteRoot + "/Forums/EditForum.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
				    Visible="<%# IsEditable %>"  />
				<portal:NoFollowHyperlink id="HyperLink3" runat="server" EnableViewState="false"
				    Tooltip="RSS" CssClass="forumfeed" 
				    NavigateUrl='<%# NonSslSiteRoot + "/Forums/RSS.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
				    Visible="<%# Config.EnableRSSAtForumLevel && !displaySettings.HideFeedLinks %>"  />
				<asp:HyperLink id="Hyperlink2" runat="server" EnableViewState="false" SkinID="TitleLink"
				    NavigateUrl='<%# FormatUrl(Convert.ToInt32(Eval("ItemID"))) %>' >
				    <%# DataBinder.Eval(Container.DataItem,"Title") %></asp:HyperLink></h3>
            </div>
            <div class="fdesc">
            <%# DataBinder.Eval(Container.DataItem,"Description").ToString()%>
            </div>
            <div class="forumstats">
                <div class="forumthreadcount">
                <mp:SiteLabel id="SiteLabel1" runat="server" ConfigKey="ForumModuleThreadCountLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
				<%# DataBinder.Eval(Container.DataItem,"ThreadCount") %>
                </div>
                <div class="forumpostcount">
                <mp:SiteLabel id="SiteLabel2" runat="server" ConfigKey="ForumModulePostCountLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
				<%# DataBinder.Eval(Container.DataItem,"PostCount") %>
                </div>
                <div class="forumlastpost">
                <mp:SiteLabel id="SiteLabel3" runat="server" ConfigKey="ForumModulePostLastPostLabel" ResourceFile="ForumResources" UseLabelTag="false" CssClass="fstatlabel" />
                 <%# FormatDate(Eval("MostRecentPostDate")) %>
                 </div>
            </div>
           <div id="divSubscribedAlt" runat="server" enableviewstate='<%# ShowSubscribeCheckboxes %>' class="fsubscribe" Visible='<%# Request.IsAuthenticated %>'>  
                <div id="divSbubcriberCount" runat="server" enableviewstate="false" visible='<%# (Config.ShowSubscriberCount &&(!IsEditable)) %>' class="forumsubstats">
                   <asp:Literal ID="litSubCount" runat="server" EnableViewState="false" Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
                </div> 
                <div id="divEditor" runat="server" enableviewstate="false" visible='<%# IsEditable %>' class="forumsubstats">
                    <asp:HyperLink ID="lnkSubscribers" runat="server" EnableViewState="false" CssClass="cblink fsubscribers" 
                    NavigateUrl='<%# SiteRoot + "/Forums/SubscriberDialog.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
                    Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' 
                    ToolTip='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
                </div>
                <div class="forumnotify">
                <asp:HyperLink ID="lnkNotify" runat="server" EnableViewState="false" CssClass="fsubcribe1 fsubscribe1" Visible='<%# !ShowSubscribeCheckboxes %>' ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/email.png"  %>' NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>' 
				 Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>' />
                 <asp:HyperLink ID="lnkNotify2" CssClass="fsubcribe2 fsubscribe2" runat="server" EnableViewState="false" Visible='<%# !ShowSubscribeCheckboxes %>' NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>' 
				 Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>'
                 ToolTip='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>' />
                 <a id='forum<%# Eval("ItemID") %>' />
					<asp:CheckBox id="chkSubscribedAlt" runat="server" Visible='<%# ShowSubscribeCheckboxes %>' Text="<%$ Resources:ForumResources, ForumModuleSubscribedLabel %>"
						Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Subscribed")) %>'
						OnCheckedChanged="Subscribed_CheckedChanged" EnableViewState="true" />
                 </div>
				
          </div>
       </li>
      </alternatingitemtemplate>
       <FooterTemplate></ul></FooterTemplate>
    </asp:repeater>
    <div id="divSubscribeButtons" runat="server" class="settingrow">
        <portal:mojoButton ID="btnSave" runat="server" Text="Save" />
        <portal:mojoButton ID="btnCancel" runat="server" Text="Cancel" />
        <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="forumeditsubscriptionshelp" />
    </div>
    <div id="divEditSubscriptions" runat="server" class="settingrow forumnotification">
        <portal:NoFollowHyperlink ID="lnkModuleRSS" runat="server" EnableViewState="false" CssClass="forumfeed forummodulefeed" />
        <asp:hyperlink id="editSubscriptionsLink" runat="server" EnableViewState="false" />
    </div>
</asp:panel>
