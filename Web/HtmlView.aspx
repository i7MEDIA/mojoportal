<%@ Page language="c#" Codebehind="HtmlView.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.ContentUI.HtmlView" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <mp:CornerRounderTop id="ctop1" runat="server" />
		<asp:Panel id="pnlHtml" runat="server" CssClass="panelwrapper htmlview">
		    <h2 class="moduletitle"><asp:Label id="lblTitle"  Runat="server"></asp:Label></h2>
		    <div class="modulecontent">
		    <div id="divHtml" runat="server" class="modulecontent"></div>
		    </div>
		</asp:Panel>
	<mp:CornerRounderBottom id="cbottom1" runat="server" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
