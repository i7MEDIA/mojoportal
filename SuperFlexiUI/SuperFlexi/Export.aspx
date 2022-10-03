<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" CodeBehind="Export.aspx.cs" Inherits="SuperFlexiUI.Export" %>

<%@ Register Namespace="SuperFlexiUI" Assembly="SuperFlexiUI" TagPrefix="flexi" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <flexi:SuperFlexiDisplaySettings ID="displaySettings" runat="server" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper flexi flexi-export">
            <portal:HeadingControl ID="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent row">
                    <div class="flexi-export-instructions col-md-6">
                        <asp:Literal ID="litInstructions" runat="server" />
                    </div>
                    <div class="col-md-6">
						<asp:TextBox ID="txtExportName" runat="server" /><br />
                        <portal:mojoButton ID="exportButton" runat="server" Text="Export" SkinID="ExportButton" UseSubmitBehavior="false" />
                        <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" SkinID="TextButton" />
                    </div>
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
