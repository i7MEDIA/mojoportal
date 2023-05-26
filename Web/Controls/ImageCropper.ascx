<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ImageCropper.ascx.cs" Inherits="mojoPortal.Web.UI.ImageCropper" %>

<asp:Panel ID="pnlCropped" runat="server">
	<div class="settingrow">
		<mp:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="CropperCroppedImage" EnableViewState="false" CssClass="settinglabel" UseLabelTag="false"></mp:SiteLabel>
	</div>
	<div class="settingrow">
		<asp:Image ID="imgCropped" runat="server" />
	</div>
</asp:Panel>
<asp:Panel ID="pnlCrop" runat="server">
	<asp:Panel ID="pnlFinalSize" runat="server" CssClass="settingrow">
		<mp:SiteLabel ID="SiteLabel2" runat="server" ConfigKey="CroppedImageWidth" EnableViewState="false" UseLabelTag="false"></mp:SiteLabel>
		<asp:TextBox ID="txtFinalWidth" runat="server" Text="0" CssClass="smalltextbox" />
		<mp:SiteLabel ID="SiteLabel3" runat="server" ConfigKey="CroppedImageHeight" EnableViewState="false" UseLabelTag="false"></mp:SiteLabel>
		<asp:TextBox ID="txtFinalHeight" runat="server" Text="0" CssClass="smalltextbox" />
		<mp:SiteLabel ID="SiteLabel4" runat="server" ConfigKey="CropperResizeInfo" EnableViewState="false" UseLabelTag="false"></mp:SiteLabel>
	</asp:Panel>
	<div class="settingrow">
		<mp:SiteLabel ID="lbl1" runat="server" ConfigKey="CropperOriginalImage" EnableViewState="false" CssClass="settinglabel" UseLabelTag="false"></mp:SiteLabel>
	</div>
	<asp:Panel ID="pnlImage" runat="server">
		<asp:Image ID="imgToCrop" runat="server" />
	</asp:Panel>
	<div class="settingrow">
		<asp:Button ID="btnCrop" runat="server" />
		<mp:SiteLabel ID="lblCroppedFileName" runat="server" ConfigKey="CropperCroppedImageFileName" EnableViewState="false" UseLabelTag="false"></mp:SiteLabel>
		<asp:TextBox ID="txtCroppedFileName" runat="server" CssClass="widetextbox" />
	</div>
	<asp:HiddenField ID="X" runat="server" />
	<asp:HiddenField ID="Y" runat="server" />
	<asp:HiddenField ID="W" runat="server" />
	<asp:HiddenField ID="H" runat="server" />
</asp:Panel>
