<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="RelatedNewsletterSetting.ascx.cs" Inherits="mojoPortal.Web.UI.RelatedNewsletterSetting" %>

<asp:UpdatePanel ID="up" UpdateMode="Conditional" runat="server" class="row">
	<ContentTemplate>
		<div class="col-sm-5 col-md-4 col-lg-3">
			<mp:SiteLabel ID="Sitelabel4" runat="server" ConfigKey="AvailableNewsletters" CssClass="newsletterpicker-label" />
			<asp:ListBox ID="lstAll" runat="Server" SelectionMode="Multiple" CssClass="newsletterpicker-list" />
		</div>
		<div class="col-sm-2 col-lg-1">
			<asp:Button Text="<" runat="server" ID="btnRemove" CausesValidation="false" />
			<asp:Button Text=">" runat="server" ID="btnAdd" CausesValidation="false" />
		</div>
		<div class="col-sm-5 col-md-4 col-lg-3">
			<mp:SiteLabel ID="Sitelabel5" runat="server" ConfigKey="SelectedNewsletters" CssClass="newsletterpicker-label" />
			<asp:ListBox ID="lstSelected" runat="Server" SelectionMode="Multiple" CssClass="newsletterpicker-list" />
		</div>
		<asp:HiddenField ID="GuidCsv" runat="server" />
	</ContentTemplate>
</asp:UpdatePanel>
