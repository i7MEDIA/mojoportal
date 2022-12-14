<%@ Page Language="c#" CodeBehind="Edit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="SuperFlexiUI.EditItems" %>
<%@ Register Namespace="SuperFlexiUI" Assembly="SuperFlexiUI" TagPrefix="flexi" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <flexi:SuperFlexiDisplaySettings ID="displaySettings" runat="server" />
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper flexi">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Panel ID="pnlEdit" runat="server" DefaultButton="updateButton">
						<asp:Panel id="customControls" runat="server"></asp:Panel>
						<div class="settingrow row_vieworder">
							<mp:SiteLabel ID="lblViewOrder" runat="server" ForControl="txtViewOrder" ConfigKey="ViewOrderLabel"
								ResourceFile="SuperFlexiResources" CssClass="settinglabel" /><asp:TextBox ID="txtViewOrder" runat="server" MaxLength="10" CssClass="forminput smalltextbox" TextMode="Number" TabIndex="10" />
						</div>
						<div class="settingrow buttonrow row_buttons"><portal:mojoButton ID="updateButton" runat="server" Text="Update" SkinID="SaveButton" /> <portal:mojoButton ID="saveAsNewButton" runat="server" Text="Save as New" SkinID="SaveAsNewButton" Visible="false" /> <portal:mojoButton ID="deleteButton" runat="server" Text="Delete this item" CausesValidation="false" SkinID="DeleteButton" /> <portal:mojoButton ID="exportButton" runat="server" Text="Export" SkinID="ExportButton" UseSubmitBehavior="false"/> <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" SkinID="TextButton" TabIndex="10" /></div>
					</asp:Panel>
					<asp:HiddenField ID="hdnReturnUrl" runat="server" />
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
		</portal:InnerWrapperPanel>

	</portal:OuterWrapperPanel>
	<portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
