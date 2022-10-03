<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="ContentPublishDialog.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ContentPublishDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
	<div style="padding: 5px 5px 5px 5px;" class="yui-skin-sam">
		<asp:Panel ID="pnlUpdate" runat="server">
			<div class="settingrow">
				<mp:SiteLabel ID="SiteLabel5" runat="server" ConfigKey="PageSettingsPageNameLabel" CssClass="settinglabel" />
				<asp:Label ID="lblPageName" runat="server" />
			</div>
			<div class="settingrow">
				<mp:SiteLabel ID="SiteLabel6" runat="server" ConfigKey="ModuleSettingsModuleNameLabel" CssClass="settinglabel" />
				<asp:Label ID="lblModuleName" runat="server" />
			</div>
			<div class="settingrow">
				<mp:SiteLabel ID="SiteLabel4" runat="server" ConfigKey="ContentManagerPublishedColumnHeader" ForControl="chkPublished" CssClass="settinglabel" />
				<asp:CheckBox ID="chkPublished" runat="server" />
			</div>

			<div class="settingrow">
				<mp:SiteLabel ID="lblPageNameLayout" runat="server" ConfigKey="ContentManagerColumnColumnHeader" ForControl="ddPaneNames" CssClass="settinglabel" />
				<asp:DropDownList ID="ddPaneNames" runat="server" DataTextField="key" DataValueField="value" CssClass="forminput"></asp:DropDownList>
			</div>
			<div class="settingrow">
				<mp:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="ContentManagerOrderColumnHeader" ForControl="txtModuleOrder" CssClass="settinglabel" />
				<asp:TextBox ID="txtModuleOrder" Columns="4" runat="server" CssClass="forminput smalltextbox" />
			</div>

			<div class="settingrow">
				<mp:SiteLabel ID="SiteLabel2" runat="server" ConfigKey="ContentManagerPublishBeginDateColumnHeader" CssClass="settinglabel" />
				<mp:DatePickerControl ID="dpBeginDate" runat="server" ShowTime="True" SkinID="ContentManager"></mp:DatePickerControl>
				<asp:RequiredFieldValidator ID="reqElement" runat="server" ControlToValidate="dpBeginDate" Display="Dynamic" />
			</div>
			<div class="settingrow">
				<mp:SiteLabel ID="SiteLabel3" runat="server" ConfigKey="ContentManagerPublishEndDateColumnHeader" CssClass="settinglabel" />
				<mp:DatePickerControl ID="dpEndDate" runat="server" ShowTime="True" SkinID="ContentManager"></mp:DatePickerControl>

			</div>
			<div class="settingrow">
				<portal:mojoButton ID="btnSave" runat="server" Text="" />
			</div>
		</asp:Panel>
		<asp:Panel ID="pnlFinished" runat="server" Visible="false">
			<script type="text/javascript">
				window.parent.location.reload();
				window.close();
			</script>

		</asp:Panel>

	</div>

</asp:Content>
