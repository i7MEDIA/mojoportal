<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="RedirectManager.aspx.cs" Inherits="mojoPortal.Web.AdminUI.RedirectManagerPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkAdvancedTools" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkRedirectManager" runat="server" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin urlmanager">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
			<asp:Panel ID="pnlAddRedirect" runat="server" DefaultButton="btnAdd">
			<div class="settingrow">
			    <asp:Label ID="lblSiteRoot" Runat="server" ></asp:Label>
				<asp:TextBox ID="txtOldUrl" Runat="server" Columns="60" MaxLength="255"></asp:TextBox>
			</div>
			<div class="settingrow">
			    <strong><mp:SiteLabel id="Sitelabel4" runat="server" ConfigKey="RedirectsToLabel" ></mp:SiteLabel></strong>
			</div>
			<div class="settingrow">
			    <asp:Label ID="lblSiteRoot2" Runat="server" ></asp:Label>
				<asp:Textbox id="txtNewUrl" Columns="60"   runat="server" />
				<portal:mojoButton  runat="server" ID="btnAdd"/> 
			</div>
			<div class="settingrow">
			     <mp:SiteLabel id="Sitelabel5" runat="server" ConfigKey="RedirectHelp" ></mp:SiteLabel>
			</div>
			<div>
		        <portal:mojoLabel ID="lblError" Runat="server" CssClass="txterror warning"></portal:mojoLabel>
		    </div>
			</asp:Panel>
		<hr />
		<portal:mojoDataList id="dlRedirects" DataKeyField="RowGuid" runat="server">
			<ItemTemplate>
				<asp:ImageButton ImageUrl='<%# EditPropertiesImage %>' CommandName="edit" AlternateText="<%# Resources.Resource.EditLink %>" ToolTip="<%# Resources.Resource.EditLink %>"  runat="server" ID="btnEdit"/>
				<asp:ImageButton ImageUrl='<%# DeleteLinkImage %>' CommandName="delete" AlternateText="<%# Resources.Resource.DeleteLink %>" ToolTip="<%# Resources.Resource.DeleteLink %>" runat="server" ID="btnDelete"/>
				&nbsp;&nbsp;
				<a href='<%# RootUrl + DataBinder.Eval(Container.DataItem, "OldUrl")%>'><%# RootUrl + DataBinder.Eval(Container.DataItem, "OldUrl")%></a> &nbsp;&nbsp;
				<strong><mp:SiteLabel id="lblPageNameLayout" runat="server" ConfigKey="RedirectsToLabel" UseLabelTag="false"></mp:SiteLabel></strong>&nbsp;&nbsp;
				<a href='<%# RootUrl + DataBinder.Eval(Container.DataItem, "NewUrl")%>'><%# RootUrl + DataBinder.Eval(Container.DataItem, "NewUrl")%></a>
			    <hr />
			</ItemTemplate>
			<EditItemTemplate>
			<fieldset>
			    <div class="settingrow">
			    <asp:Label Text='<%# RootUrl %>'  runat="server" ID="Label3"/><asp:Textbox id="txtGridOldUrl" Columns="60" Text='<%# DataBinder.Eval(Container.DataItem, "OldUrl").ToString() %>' runat="server" />
			    </div>
			    <div class="settingrow">
			        <strong><mp:SiteLabel id="Sitelabel4" runat="server" ConfigKey="RedirectsToLabel" ></mp:SiteLabel></strong>
			    </div>
			    <div class="settingrow">
			    <asp:Label Text='<%# RootUrl %>'  runat="server" ID="Label4"/>
				<asp:Textbox id="txtGridNewUrl" Columns="60" Text='<%# DataBinder.Eval(Container.DataItem, "NewUrl").ToString() %>' runat="server" />
				 
				</div>
				<div class="settingrow">
				    <asp:Button Text="<%# Resources.Resource.SaveButton %>" ToolTip="<%# Resources.Resource.SaveButton %>" CommandName="apply" runat="server" ID="button1" />
				    <asp:Button Text="<%# Resources.Resource.CancelButton %>" ToolTip="<%# Resources.Resource.CancelButton %>" CommandName="cancel" runat="server" ID="button3" /> 
				</div>
		    </fieldset>
			</EditItemTemplate>
		</portal:mojoDataList>
    <portal:mojoCutePager ID="pgrFriendlyUrls" runat="server" Visible="false" />
</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
