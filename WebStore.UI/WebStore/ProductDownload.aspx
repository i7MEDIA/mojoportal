<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    Codebehind="ProductDownload.aspx.cs" Inherits="WebStore.UI.ProductDownloadPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<asp:Panel ID="pnlExpiredDownload" runat="server" Visible="false">
<mp:SiteLabel ID="SiteLabel12" runat="server" UseLabelTag="false" ConfigKey="InvalidOrExpiredDownloadWarning" ResourceFile="WebStoreResources" />
</asp:Panel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
