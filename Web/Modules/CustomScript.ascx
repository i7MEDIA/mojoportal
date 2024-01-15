<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomScript.ascx.cs" Inherits="mojoPortal.Web.UI.CustomScriptModule" %>
<portal:ModuleTitleControl ID="TitleTop" runat="server" EnableViewState="false" Visible="false" />
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper customscriptmodule">
		<portal:ModuleTitleControl ID="Title1" runat="server" EnableViewState="false" />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
				<asp:Literal ID="litScriptUrl" runat="server" EnableViewState="false" />
				<asp:Literal ID="litScript" runat="server" EnableViewState="false" />
			</portal:InnerBodyPanel>
			
		</portal:OuterBodyPanel>
	</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
