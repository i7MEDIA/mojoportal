<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="UrlManager.aspx.cs" Inherits="mojoPortal.Web.AdminUI.UrlManagerPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkAdvancedTools" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkUrlManager" runat="server" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin urlmanager">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">

<asp:Panel ID="pnlWrapper" runat="server" CssClass="panelwrapper admin urlmanager">
<portal:HeadingControl ID="subHeading" runat="server" SkinID="subhead" HeadingTag="h3" />
			<asp:Panel ID="pnlAddUrl" runat="server" DefaultButton="btnAddFriendlyUrl">
			<div class="settingrow">
			    <asp:Label ID="lblFriendlyUrlRoot" Runat="server" ></asp:Label>
				<asp:TextBox ID="txtFriendlyUrl" Runat="server" Columns="60" MaxLength="255"></asp:TextBox>
			</div>
			<div class="settingrow">
			    <strong><mp:SiteLabel id="Sitelabel4" runat="server" ConfigKey="FriendlyUrlMapsToLabel" ></mp:SiteLabel></strong>
			</div>
			<div class="settingrow">
			    <mp:SiteLabel id="Sitelabel7" runat="server" ConfigKey="FriendlyUrlSelectFromDropdownLabel" CssClass="settinglabel"></mp:SiteLabel>
			    <asp:DropDownList id="ddPages" runat="server" DataTextField="key" DataValueField="value"></asp:DropDownList>
			    <portal:mojoButton  runat="server" id="btnAddFriendlyUrl" />
			</div>
			<div class="settingrow">
			    <mp:SiteLabel id="Sitelabel6" runat="server" ConfigKey="FriendlyUrlExpertEntryLabel" CssClass="settinglabel"></mp:SiteLabel>
				<asp:Textbox id="txtRealUrl" Columns="60"   runat="server" />
				<portal:mojoButton  runat="server" ID="btnAddExpert" /> 
			</div>
			<div class="settingrow">
			    <portal:mojoHelpLink ID="MojoHelpLink4" runat="server" HelpKey="friendlyurlhelp" />	
			</div>
			<div>
		        <portal:mojoLabel ID="lblError" Runat="server" CssClass="txterror warning" />
		    </div>
			</asp:Panel>
			
		
		<asp:Panel ID="pnlSearch" runat="server" CssClass="settingrow urlsearch" DefaultButton="btnSearchUrls">
		    <asp:TextBox ID="txtSearch" runat="server" CssClass="widetextbox" />
		    <portal:mojoButton ID="btnSearchUrls" runat="server" />
		</asp:Panel>
		<portal:mojoDataList id="dlUrlMap" DataKeyField="UrlID" runat="server">
			<ItemTemplate>
				<asp:ImageButton ImageUrl='<%# EditPropertiesImage %>' CommandName="edit" AlternateText="<%# Resources.Resource.FriendlyUrlEditLabel %>" ToolTip="<%# Resources.Resource.FriendlyUrlEditLabel %>"  runat="server" ID="btnEditUrl"/>
				<asp:ImageButton ImageUrl='<%# DeleteLinkImage %>' CommandName="delete" AlternateText="<%# Resources.Resource.FriendlyUrlDeleteLabel %>" ToolTip="<%# Resources.Resource.FriendlyUrlDeleteLabel %>" runat="server" ID="btnDeleteUrl"/>
				&nbsp;&nbsp;
				<asp:Label Text='<%# RootUrl %>'  runat="server" ID="Label2"/><%# DataBinder.Eval(Container.DataItem, "FriendlyUrl") %><br />
				<strong><mp:SiteLabel id="lblPageNameLayout" runat="server" ConfigKey="FriendlyUrlMapsToLabel" UseLabelTag="false"></mp:SiteLabel></strong>
				<%# RootUrl %><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "RealUrl").ToString().Replace("~/","")) %>
				<a href='<%# RootUrl + DataBinder.Eval(Container.DataItem, "FriendlyUrl") %>'>
					<mp:SiteLabel id="Sitelabel1" runat="server" ConfigKey="FriendlyUrlViewLink" UseLabelTag="false"></mp:SiteLabel></a>
			    <hr />
			</ItemTemplate>
			<EditItemTemplate>
			<fieldset>
			    <div class="settingrow">
			    <h3><mp:SiteLabel id="Sitelabel7" runat="server" ConfigKey="FriendlyUrlSelectFromDropdownLabel" UseLabelTag="false"></mp:SiteLabel></h3>
			    </div>
			    <div class="settingrow">
			        <asp:Textbox id="txtItemFriendlyUrl" Columns="60" Text='<%# DataBinder.Eval(Container.DataItem, "FriendlyUrl") %>' runat="server" />
				&nbsp;
				<strong><mp:SiteLabel id="Sitelabel2" runat="server" ConfigKey="FriendlyUrlMapsToLabel" UseLabelTag="false"></mp:SiteLabel></strong>
				<asp:DropDownList ID="ddPagesEdit" runat="server" DataSource='<%# PageList %>'  DataValueField="value" DataTextField="key" />
			    <asp:Button Text="<%# Resources.Resource.FriendlyUrlSaveLabel%>" ToolTip="<%# Resources.Resource.FriendlyUrlSaveLabel%>" CommandName="apply" runat="server" ID="button2"/> 
			    </div>
			    <div class="settingrow">
			    <h3><mp:SiteLabel id="Sitelabel6" runat="server" ConfigKey="FriendlyUrlExpertEntryLabel" UseLabelTag="false"></mp:SiteLabel></h3>
			    </div>
			    <div class="settingrow">
				<asp:Textbox id="txtItemRealUrl" Columns="60" Text='<%# DataBinder.Eval(Container.DataItem, "RealUrl").ToString().Replace("~/","") %>' runat="server" />
				<asp:Button Text="<%# Resources.Resource.FriendlyUrlSaveLabel %>" ToolTip="<%# Resources.Resource.FriendlyUrlSaveLabel %>" CommandName="applymanual" runat="server" ID="button1"/> 
				</div>
				<div class="settingrow">
				    <asp:Button Text="<%# Resources.Resource.FriendlyUrlCancelButton %>" ToolTip="<%# Resources.Resource.FriendlyUrlCancelButton %>" CommandName="cancel" runat="server" ID="button3"/> 
				</div>
		    
			</EditItemTemplate>
		</portal:mojoDataList>
    <portal:mojoCutePager ID="pgrFriendlyUrls" runat="server" />
</asp:Panel>
</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
    runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" 
    runat="server" >
</asp:Content>

