<%@ Control Language="c#" Inherits="mojoPortal.Web.ContentUI.HtmlModule" CodeBehind="HtmlModule.ascx.cs" AutoEventWireup="false" %>

<%@ Register Namespace="mojoPortal.Web.ContentUI" Assembly="mojoPortal.Web" TagPrefix="html" %>

<html:htmldisplaysettings id="displaySettings" runat="server" />

<portal:outerwrapperpanel id="pnlOuterWrap" runat="server">
	<portal:innerwrapperpanel id="pnlInnerWrap" runat="server" cssclass="panelwrapper htmlmodule">
		<portal:moduletitlecontrol id="Title1" runat="server" editurl="/Modules/HtmlEdit.aspx" enableviewstate="false" />

		<portal:outerbodypanel id="pnlOuterBody" runat="server">
			<portal:innerbodypanel id="pnlInnerBody" runat="server" cssclass="modulecontent">
				<portal:mojorating runat="server" id="Rating" enabled="false" />

				<portal:slidepanel id="divContent" runat="server" enableviewstate="false" enableslideshow="false" class="slidecontainer"></portal:slidepanel>

				<asp:HiddenField ID="hdnIsDirty" runat="server" />

				<asp:Panel ID="pnlAuthorInfo" runat="server" EnableViewState="false" CssClass="authorinfo">
					<portal:avatar id="userAvatar" runat="server" />

					<span id="spnAuthorBio" runat="server" visible="false" enableviewstate="false" class="authorbio"></span>
				</asp:Panel>

				<asp:Panel ID="pnlCreatedBy" runat="server" CssClass="authorinfo createdby" Visible="false">
					<asp:Literal ID="litCreatedBy" runat="server" />
				</asp:Panel>

				<asp:Panel ID="pnlModifiedBy" runat="server" CssClass="authorinfo modifiedby" Visible="false">
					<asp:Literal ID="litModifiedBy" runat="server" />
				</asp:Panel>

				<portal:mojorating runat="server" id="RatingBottom" enabled="false" />

				<portal:facebooklikebutton id="fbLike" runat="server" visible="false" />
			</portal:innerbodypanel>

			<portal:emptypanel id="divFooter" runat="server" cssclass="modulefooter" skinid="modulefooter"></portal:emptypanel>
		</portal:outerbodypanel>
	</portal:innerwrapperpanel>
</portal:outerwrapperpanel>
