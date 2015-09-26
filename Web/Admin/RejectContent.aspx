<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="RejectContent.aspx.cs" Inherits="mojoPortal.Web.AdminUI.RejectContent" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlWrapper" runat="server" CssClass="panelwrapper editpage admin htmlmodule workflow ">
        <asp:Panel ID="pnlReject" runat="server" CssClass="modulecontent" DefaultButton="btnUpdate">
            <fieldset class="htmledit">
                <legend>
                    <asp:Literal ID="litModuleTitle" runat="server" />
                </legend>
                <div id="divRejectionIntro" runat="server" class="settingrow">
                    <asp:Literal ID="litRejectionIntroduction" runat="server" />
                </div>
                <div id="divRejectionComments" runat="server" class="settingrow">
                    <mp:SiteLabel ID="lblRejectionComments" runat="server" CssClass="settinglabel" ConfigKey="RejectionCommentLabel">
                    </mp:SiteLabel>
                    <asp:TextBox ID="txtRejectionComments" runat="server" TextMode="MultiLine" Width="400"
                        Height="250"></asp:TextBox>
                </div>
                <div class="settingrow">
                    <portal:mojoButton ID="btnUpdate" runat="server" Text="" OnClick="btnUpdate_Click" />&nbsp;
                    <portal:mojoButton ID="btnCancel" runat="server" Text="" OnClick="btnCancel_Click" />
                </div>
            </fieldset>
        </asp:Panel>
    </asp:Panel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
