<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    Codebehind="OrderDownloads.aspx.cs" Inherits="WebStore.UI.OrderDownloadsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <mp:CornerRounderTop id="ctop1" runat="server" />
    <asp:Panel ID="pnlStoreManager" runat="server" CssClass="panelwrapper webstore webstoredownloads">
    <div class="modulecontent">
    <asp:Panel ID="pnlOrderDownloads" runat="server">
    </asp:Panel>
    </div>
    </asp:Panel>
    <mp:CornerRounderBottom id="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
