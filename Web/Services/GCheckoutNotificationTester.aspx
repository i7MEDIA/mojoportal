<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="GCheckoutNotificationTester.aspx.cs" Inherits="mojoPortal.Web.UI.GCheckoutNotificationTesterPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

<div class="settingrow">
<mp:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="GCheckoutNotificationHandlerUrlLabel"
                ResourceFile="Resource" />
</div>
<div class="settingrow">
<asp:TextBox ID="txtNotificationHandlerUrl" runat="server" Columns="80" />
<asp:Button ID="btnPost" runat="server" />
</div>
<div class="settingrow">
<mp:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="GCheckoutXmlInputLabel"
                ResourceFile="Resource" />
</div>
<div class="settingrow">
<asp:TextBox ID="txtXmlInput" runat="server" TextMode="MultiLine" Rows="20" Columns="60" />
</div>
<div class="settingrow">
<mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="GCheckoutXmlResultsLabel"
                ResourceFile="Resource" />
</div>
<div class="settingrow">
<asp:TextBox ID="txtReponse" runat="server" TextMode="MultiLine" Rows="5" Columns="60" />
</div>


</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
