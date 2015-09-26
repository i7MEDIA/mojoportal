<%@ Page Language="c#" CodeBehind="EditImage.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="mojoPortal.Web.GalleryUI.GalleryImageEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
   <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper gallerymodule">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
        <asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate">
                <div class="settingrow">
                    <mp:SiteLabel ID="Sitelabel2" ForControl="fckDescription" CssClass="settinglabel"
                        runat="server" ConfigKey="GalleryDescriptionLabel" ResourceFile="GalleryResources">
                    </mp:SiteLabel>
                    <br />
                </div>
                <div class="settingrow">
                    <mpe:EditorControl ID="edDescription" runat="server">
                    </mpe:EditorControl>
                </div>
                <div class="settingrow">
                    <mp:SiteLabel ID="Sitelabel1" runat="server" ForControl="txtCaption" CssClass="settinglabel"
                        ConfigKey="GalleryCaptionLabel" ResourceFile="GalleryResources"></mp:SiteLabel>
                    <asp:TextBox ID="txtCaption" runat="server" MaxLength="255" Columns="50" CssClass="widetextbox forminput"></asp:TextBox>
                </div>
                <div class="settingrow">
                    <mp:SiteLabel ID="lblDisplayOrder" runat="server" ForControl="txtDisplayOrder" CssClass="settinglabel"
                        ConfigKey="GalleryDisplayOrderLabel" ResourceFile="GalleryResources"></mp:SiteLabel>
                    <asp:TextBox ID="txtDisplayOrder" runat="server" MaxLength="10" Text="100" CssClass="smalltextbox forminput"></asp:TextBox>
                </div>
                <div class="settingrow">
                    <img alt=" " id="imgThumb" runat="server" src="/Data/SiteImages/1x1.gif" />
                </div>
                <div class="settingrow">  
                    <portal:jQueryFileUpload ID="uploader" runat="server" />
                    <asp:HiddenField ID="hdnState" Value="" runat="server" />
                </div>
                <div class="settingrow">
                    <portal:mojoLabel ID="lblMessage" runat="server" CssClass="txterror" />
                </div>
                <div class="settingrow">
                    <mp:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                    <div class="forminput">
                        <portal:mojoButton ID="btnUpdate" runat="server" />&nbsp;&nbsp;
                        <portal:mojoButton ID="btnDelete" runat="server" CausesValidation="false" />&nbsp;&nbsp;
                        <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;
                        <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="galleryedithelp" /><br />
                        <asp:RegularExpressionValidator id="regexFile" 
				            ControlToValidate="uploader"
				            ValidationExpression="(([^.;]*[.])+(jpg|gif|png|JPG|GIF|PNG); *)*(([^.;]*[.])+(jpg|gif|png|JPG|GIF|PNG))?$"
				            Display="Static"
				            EnableClientScript="True" 
				            runat="server"/>
                    </div>
                </div>
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
