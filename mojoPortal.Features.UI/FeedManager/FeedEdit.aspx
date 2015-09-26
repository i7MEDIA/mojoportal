<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="FeedEdit.aspx.cs" Inherits="mojoPortal.Web.FeedUI.FeedEditPage" %>

<%@ Register TagPrefix="mpf" TagName="FeedTypeSetting" Src="~/FeedManager/Controls/FeedTypeSetting.ascx" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper rssmodule">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate">

	        <div class="settingrow">
	            <mp:SiteLabel id="lblTitleLabel" runat="server" ForControl="txtAuthor" CssClass="settinglabel" ConfigKey="AuthorLabel" ResourceFile="FeedResources" ></mp:SiteLabel>
	            <asp:TextBox id="txtAuthor" runat="server" maxlength="100"  CssClass="forminput widetextbox"></asp:TextBox>
	        </div>
	        <div class="settingrow">
	            <mp:SiteLabel id="Sitelabel1" runat="server" ForControl="txtWebSite" CssClass="settinglabel" ConfigKey="WebSiteLabel" ResourceFile="FeedResources" ></mp:SiteLabel>
	            <asp:TextBox id="txtWebSite" runat="server" maxlength="255"  CssClass="forminput widetextbox"></asp:TextBox>
	        </div>
	        <div class="settingrow">
	            <mp:SiteLabel id="Sitelabel2" runat="server" ForControl="txtRssUrl" CssClass="settinglabel" ConfigKey="FeedUrlLabel" ResourceFile="FeedResources" ></mp:SiteLabel>
	            <asp:TextBox id="txtRssUrl" runat="server" maxlength="1000"  CssClass="forminput widetextbox"></asp:TextBox>
	        </div>
	        <div class="settingrow">
	            <mp:SiteLabel id="Sitelabel4" runat="server" ForControl="txtSortRank" CssClass="settinglabel" ConfigKey="SortRankLabel" ResourceFile="FeedResources" ></mp:SiteLabel>
	            <asp:TextBox id="txtSortRank" runat="server" maxlength="10" Text="500"  CssClass="forminput smalltextbox"></asp:TextBox>
	        </div>
	        <div id="divImage" runat="server" visible="false" class="settingrow">
	            <mp:SiteLabel id="Sitelabel3" runat="server" ForControl="txtImageUrl" CssClass="settinglabel" ConfigKey="ImageUrlLabel" ResourceFile="FeedResources" ></mp:SiteLabel>
	            <asp:TextBox id="txtImageUrl" runat="server" maxlength="255"  CssClass="forminput widetextbox"></asp:TextBox>
	        </div>
	        <div id="divPublish" runat="server" class="settingrow">
	            <mp:SiteLabel id="Sitelabel5" runat="server" ForControl="txtRssUrl" CssClass="settinglabel" ConfigKey="PublishByDefaultLabel" ResourceFile="FeedResources" ></mp:SiteLabel>
	            <asp:CheckBox id="chkPublishByDefault" runat="server" CssClass="forminput" />
	        </div>
	        <div class="settingrow">
	            <asp:Label id="lblError" runat="server" ></asp:Label>
	        </div>
	        <div class="settingrow">
        <mp:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
            <div class="forminput">
	            <portal:mojoButton  id="btnUpdate" runat="server" Text="Update" ValidationGroup="feeds" />&nbsp;
			    <portal:mojoButton  id="btnDelete" runat="server" Text="" CausesValidation="false" />&nbsp;
			    <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;	
			    <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="rssfeededithelp" />
			    </div>
			    <asp:ValidationSummary id="vSummary" runat="server" ValidationGroup="feeds" />
			    <asp:RequiredFieldValidator id="reqTitle" runat="server" ControlToValidate="txtAuthor" Display="None" ValidationGroup="feeds" />
			    
			    <asp:RequiredFieldValidator id="reqFeedUrl" runat="server" ControlToValidate="txtRssUrl" Display="None" ValidationGroup="feeds" />
			    <asp:RegularExpressionValidator id="regexWebSiteUrl" runat="server" ControlToValidate="txtWebSite" Display="None" ValidationGroup="feeds" />
			    <asp:RegularExpressionValidator id="regexFeedUrl" runat="server" ControlToValidate="txtRssUrl" Display="None" ValidationGroup="feeds" />
			    
	        </div>
<asp:HiddenField ID="hdnReturnUrl" runat="server" />
</asp:Panel>
<asp:Panel ID="divNav" runat="server" CssClass="rssnavright" SkinID="plain">
    <asp:Label ID="lblFeedListName" Font-Bold="True" runat="server"></asp:Label>
    <br />
    <asp:Hyperlink id="lnkNewFeed" runat="server" />
    <portal:mojoDataList ID="dlstFeedList" runat="server" EnableViewState="false">
        <ItemTemplate>
            <asp:HyperLink ID="editLink" runat="server" Text="<%# Resources.FeedResources.EditImageAltText%>"
                ToolTip="<%# Resources.FeedResources.EditImageAltText%>" ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
                NavigateUrl='<%# this.SiteRoot + "/FeedManager/FeedEdit.aspx?pageid=" + PageId.ToString() + "&amp;ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&amp;mid=" + ModuleId.ToString()  %>'
                    />
            <asp:HyperLink ID="Hyperlink2" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Url")%>'>
				<%# DataBinder.Eval(Container, "DataItem.Author")%>
            </asp:HyperLink>
                    
            <asp:HyperLink ID="Hyperlink3" runat="server" 
                ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + RssImageFile %>' 
                NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.RssUrl")%>'>
            </asp:HyperLink>&nbsp;&nbsp;
        </ItemTemplate>
    </portal:mojoDataList>
</asp:Panel>
<portal:mojoButton  id="btnClearCache" runat="server"  CausesValidation="False" />


</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

