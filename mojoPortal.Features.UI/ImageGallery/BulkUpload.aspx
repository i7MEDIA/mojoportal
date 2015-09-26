<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="BulkUpload.aspx.cs" Inherits="mojoPortal.Web.GalleryUI.BulkUploadPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper bulkimage">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<div class="settingrow">
     <portal:jQueryFileUpload ID="uploader" runat="server" />
    <asp:RegularExpressionValidator ID="regexUpload" ControlToValidate="uploader" ValidationExpression="(([^.;]*[.])+(jpg|gif|png|JPG|GIF|PNG); *)*(([^.;]*[.])+(jpg|gif|png|JPG|GIF|PNG))?$"
    Display="dynamic" ErrorMessage="Only jpg, gif, and png extensions allowed" EnableClientScript="True"
    runat="server" />
</div>
<div class="settingrow">
<portal:mojoButton ID="btnUpload" runat="server" />
    <asp:HiddenField ID="hdnState" Value="images" runat="server" />
</div>           
<div class="settingrow">
<asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;
<asp:Label ID="lblError" runat="server" CssClass="txterror"></asp:Label>
<asp:HiddenField ID="hdnReturnUrl" runat="server" />
</div>
</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
