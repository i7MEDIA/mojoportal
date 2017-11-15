<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" CodeBehind="Import.aspx.cs" Inherits="SuperFlexiUI.Import" %>
<%@ Register Namespace="SuperFlexiUI" Assembly="SuperFlexiUI" TagPrefix="flexi" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <flexi:superflexidisplaysettings id="displaySettings" runat="server" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper flexi flexi-import">
            <portal:HeadingControl ID="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                    <div class="flexi-import-instructions"><asp:Literal ID="litInstructions" runat="server" /></div>
                    <div class="form-group">
                        <mp:SiteLabel ID="lblUpdate" runat="server" ResourceFile="SuperFlexiResources" ConfigKey="ImportUpdateExistingRecordsLabel" />
                        <asp:CheckBox ID="chkUpdate" runat="server" />
                        <mp:HelpLinkButton ID="hlpUpdate" runat="server" HelpKey="flexi-ImportUpdateExistingRecords" />
                    </div>
					<div class="form-group">
						<mp:SiteLabel ID="lblDeleteAll" runat="server" ResourceFile="SuperFlexiResources" ConfigKey="ImportDeleteExistingRecordsLabel" />
						<asp:CheckBox ID="chkDelete" runat="server" />
						<mp:HelpLinkButton ID="hlpDelete" runat="server" HelpKey="flexi-ImportDeleteExistingRecords" />
					</div>
                    <div class="form-group">
                        <portal:jQueryFileUpload ID="uploader" runat="server" />
                        <portal:mojoButton ID="importButton" runat="server" Text="Import Records" SkinID="SaveButton" />
                        <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" SkinID="TextButton" />
                    </div>
                    <div class="flexi-import-results"><asp:Literal ID="litResults" runat="server" /></div>
                    <asp:HiddenField ID="hdnReturnUrl" runat="server" />
                </portal:InnerBodyPanel>
            </portal:OuterBodyPanel>
            <portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
        </portal:InnerWrapperPanel>

    </portal:OuterWrapperPanel>

    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
