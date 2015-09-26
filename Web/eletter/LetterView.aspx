<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="LetterView.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.LetterViewPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<div class="modulecontent">
<portal:mojoLabel ID="lblMessage" runat="server" CssClass="txterror info" />
<asp:Literal ID="litLoginMessage" runat="server" />
</div>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
