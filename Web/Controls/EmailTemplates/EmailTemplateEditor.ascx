<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EmailTemplateEditor.ascx.cs" Inherits="mojoPortal.Web.UI.EmailTemplateEditor" %>

<div class="settingrow">
	<asp:DropDownList ID="ddTemplates" runat="server" DataValueField="Guid" DataTextField="Name" />
	<portal:mojoButton ID="btnLoad" runat="server" />
	<asp:HyperLink ID="lnkNew" runat="server" />
</div>

<div class="settingrow">
	<mp:SiteLabel id="SiteLabel2" runat="server" ForControl="txtName" CssClass="settinglabel" ConfigKey="Title" ResourceFile="Resource" />
	<asp:TextBox ID="txtName" CssClass="verywidetextbox forminput" MaxLength="255" runat="server" />	
</div>

<div id="divSubject" runat="server" class="settingrow">
	<mp:SiteLabel id="lblSubject" runat="server" ForControl="txtSubject" CssClass="settinglabel" ConfigKey="Subject" ResourceFile="Resource" />
	<asp:TextBox ID="txtSubject" CssClass="verywidetextbox forminput" MaxLength="255" runat="server" />	
</div>


<div id="divtabs" class="mojo-tabs">
	<ul>
		<li class="selected"><a href="#tabHtml">
			<asp:Literal ID="litHtmlTab" runat="server" /></a></li>
		<li><a href="#tabPlainText">
			<asp:Literal ID="litPlainTextTab" runat="server" /></a></li>
	</ul>
	<div id="tabHtml">
		<mpe:EditorControl ID="edTemplate" runat="server"></mpe:EditorControl>
	</div>
	<div id="tabPlainText">
		<asp:TextBox ID="txtPlainText" runat="server" TextMode="MultiLine" Width="100%" Height="800px"></asp:TextBox>
	</div>
</div>

<asp:Panel ID="pnlTokens" runat="server">
	<div class="settingrow">
		<mp:SiteLabel id="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="SupportedTokens" ResourceFile="Resource" UseLabelTag="false" />
		<portal:mojoHelpLink ID="hlpTokens" runat="server" />
	</div>
	<div class="settingrow">
		<asp:Literal ID="litTokens" runat="server" />
	</div>
</asp:Panel>

<div class="settingrow">
	<portal:mojoButton ID="btnSave" runat="server" ValidationGroup="Template" />
	<portal:mojoButton ID="btnDelete" runat="server" CausesValidation="false" />
</div>

<div class="settingrow">
	<asp:RequiredFieldValidator ID="reqTitle" runat="server" ControlToValidate="txtName" Display="None" ValidationGroup="Template" />
	<asp:RequiredFieldValidator ID="reqSubject" runat="server" ControlToValidate="txtSubject" Display="None" ValidationGroup="Template" />
	<asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="Template" />
</div>