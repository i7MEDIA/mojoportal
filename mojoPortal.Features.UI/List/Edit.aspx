<%@ Page Language="c#" CodeBehind="Edit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="mojoPortal.Web.LinksUI.EditLinks" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper linksmodule">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <asp:Panel ID="pnlEdit" runat="server" DefaultButton="updateButton">
            <ol class="formlist">
            <li class="settingrow">
                <mp:SiteLabel ID="lblDescription" runat="server" ForControl="fckDescription" ConfigKey="EditLinksDescriptionLabel"
                    ResourceFile="LinkResources" CssClass="settinglabel"></mp:SiteLabel>
            </li>
            <li class="settingrow">
                <mpe:EditorControl ID="edDescription" runat="server">
                </mpe:EditorControl>
            </li>
            <li class="settingrow">
                <mp:SiteLabel ID="lblTitle" runat="server" ForControl="txtTitle" ConfigKey="EditLinksTitleLabel"
                    ResourceFile="LinkResources" CssClass="settinglabel"></mp:SiteLabel>
                <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
            </li>
            <li class="settingrow">
                <mp:SiteLabel ID="lblUrlLabel" runat="server" ForControl="txtUrl" ConfigKey="EditLinksUrlLabel"
                    ResourceFile="LinkResources" CssClass="settinglabel"></mp:SiteLabel>
                <div class="forminput">
                    <asp:DropDownList ID="ddProtocol" runat="server" EnableTheming="false">
                        <asp:ListItem Text="http://" Value="http://"></asp:ListItem>
                        <asp:ListItem Text="https://" Value="https://"></asp:ListItem>
                        <asp:ListItem Text="~/" Value="~/"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtUrl" runat="server" MaxLength="1000"  CssClass="verywidetextbox "></asp:TextBox>
                    <portal:FileBrowserTextBoxExtender id="fileBrowser" runat="server" />
                </div>
            </li>
            <li class="settingrow">
                <mp:SiteLabel ID="lblViewOrder" runat="server" ForControl="txtViewOrder" ConfigKey="EditLinksViewOrderLabel"
                    ResourceFile="LinkResources" CssClass="settinglabel"></mp:SiteLabel>
                <asp:TextBox ID="txtViewOrder" runat="server" MaxLength="10"  Text="500"
                    CssClass="forminput smalltextbox"></asp:TextBox>
            </li>
            <li class="settingrow">
                <mp:SiteLabel ID="SiteLabel1" runat="server" ForControl="chkUseNewWindow" ConfigKey="LinksUseNewWindowLabel"
                    ResourceFile="LinkResources" CssClass="settinglabel"></mp:SiteLabel>
                <asp:CheckBox ID="chkUseNewWindow" runat="server" CssClass="forminput" />
            </li>  
            <li class="settingrow">
                <asp:ValidationSummary ID="vSummary" runat="server" CssClass="txterror"></asp:ValidationSummary>
                <portal:mojoLabel ID="lblMessage" runat="server" CssClass="txterror"  />
                <asp:RequiredFieldValidator ID="reqTitle" runat="server" CssClass="txterror" ControlToValidate="txtTitle"
                    ErrorMessage="" Display="None"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="reqUrl" runat="server" CssClass="txterror" ControlToValidate="txtUrl"
                    ErrorMessage="" Display="none"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="reqViewOrder" runat="server" CssClass="txterror"
                    ControlToValidate="txtViewOrder" ErrorMessage="" Display="none"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="VerifyViewOrder" runat="server" CssClass="txterror" ControlToValidate="txtViewOrder"
                    ErrorMessage="" Display="None" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
            </li>
            <li class="settingrow ">
                <div class="forminput">
                    <portal:mojoButton ID="updateButton" runat="server" Text="Update" />&nbsp;
                    <portal:mojoButton ID="deleteButton" runat="server" Text="Delete this item" CausesValidation="false" />&nbsp;
                    <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />
                        
                </div>
            </li>
            </ol>
    </asp:Panel>
    <asp:HiddenField ID="hdnReturnUrl" runat="server" />
    </portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
