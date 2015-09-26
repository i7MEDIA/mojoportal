<%@ Page language="c#" Codebehind="Edit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.ContentUI.HtmlIncludeEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper htmlincludemodule">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate">
    <div class="settingrow">
        <mp:SiteLabel id="lblIncludeFile" runat="server" ForControl="ddInclude" CssClass="settinglabel" ConfigKey="HtmlFragmentIncludeFileLabel" ResourceFile="HtmlIncludeResources"></mp:SiteLabel>
        <asp:DropDownList ID="ddInclude" Runat="server" DataValueField="Name" DataTextField="Name"></asp:DropDownList>
    </div>
    <div class="settingrow">
    <mp:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
        <portal:mojoButton  id="btnUpdate" runat="server" Text="Update" />&nbsp;
		<portal:mojoButton  id="btnCancel" runat="server" Text="Cancel"  CausesValidation="false" />
		<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="htmlincludeedithelp" />
    </div>
</asp:Panel>
<asp:HiddenField ID="hdnReturnUrl" runat="server" />	
</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

