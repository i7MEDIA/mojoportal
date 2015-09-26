<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SkinFileEditor.aspx.cs" Inherits="mojoPortal.Web.AdminUI.SkinFileEditorPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:mojoPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<portal:mojoPanel ID="MojoPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">


</div>
</portal:mojoPanel>
 <div class="cleared"></div>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:mojoPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
