<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="AvatarUploadDialog.aspx.cs" Inherits="mojoPortal.Web.Dialog.AvatarUploadDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">

	<portal:ImageCropper ID="cropper" runat="server" />
	<asp:Label ID="lblMaxAvatarSize" runat="server" />
	<portal:mojoLabel ID="lblUploadNewAvatar" runat="server" CssClass="h2" />
	<portal:jQueryFileUpload ID="uploader" runat="server" />
	<asp:HiddenField ID="hdnState" Value="images" runat="server" />
	<asp:Button ID="btnUploadAvatar" runat="server" Text="Upload" ValidationGroup="avatar"></asp:Button>
	<asp:RegularExpressionValidator ID="regexAvatarFile" ControlToValidate="uploader"
		Display="Dynamic" EnableClientScript="True" runat="server" ValidationGroup="avatar" />
</asp:Content>
