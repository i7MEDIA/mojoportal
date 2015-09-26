<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SurveyPageEdit.aspx.cs" Inherits="SurveyFeature.UI.PageEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkSurveys" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkPages" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkPageEdit" CssClass="selectedcrumb"></asp:HyperLink>
</div>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper survey">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<asp:Panel ID="pnlPageEdit" runat="server" DefaultButton="btnSave">
    <div class="settingrow">
        <mp:SiteLabel ID="lblPageTitleLabel" runat="server" ConfigKey="PageTitleLabel" ResourceFile="SurveyResources" CssClass="settinglabel" />
        <asp:TextBox runat="server" ID="txtPageTitle" />
    </div>
    <div class="settingrow">
        <mp:SiteLabel ID="lblPageEnabledLabel" runat="server" ConfigKey="PageEnabledLabel" ResourceFile="SurveyResources" CssClass="settinglabel" />
        <asp:CheckBox runat="server" ID="chkPageEnabled" Checked="true" />
    </div>
    <div class="settingrow">
        <br />
        <portal:mojoButton ID="btnSave" runat="server" />
        <portal:mojoButton ID="btnCancel" runat="server" />
    </div>
 </asp:Panel>		
</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
<portal:SessionKeepAliveControl id="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
