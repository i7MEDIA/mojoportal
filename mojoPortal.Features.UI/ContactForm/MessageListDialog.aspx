<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="MessageListDialog.aspx.cs" Inherits="mojoPortal.Web.ContactUI.MessageListDialog" %>

<%@ Register Namespace="mojoPortal.Web.ContactUI" Assembly="mojoPortal.Features.UI" TagPrefix="contact" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
	<contact:ContactFormDisplaySettings runat="server" ID="displaySettings" />
	<link href="https://fonts.googleapis.com/css?family=Lato:300,300i,400,400i,700,700i" rel="stylesheet">
	<link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css">
	<script type="text/javascript">
		function GetMessage(messageGuid, context) {
			<%= sCallBackFunctionInvocation %>
		}

		function ShowMessage(message, context) {
			document.getElementById('<%= pnlMessage.ClientID %>').innerHTML = message;
		}

		function OnError(message, context) {
			//alert('An unhandled exception has occurred:\n' + message);
		}
	</script>

	<portal:BasePanel ID="pnlContainer" runat="server" CssClass="container-fluid" RenderId="false">
		<portal:BasePanel runat="server" ID="rowPnl" RenderId="false" CssClass="row cf-message-viewer">
			<portal:BasePanel ID="pnlLeft" runat="server" CssClass="col-sm-4" RenderId="false">
				<mp:mojoGridView runat="server"
					ID="grdContactFormMessage"
					AllowPaging="false"
					AllowSorting="false"
					CssClass=""
					TableCssClass="jqtable"
					AutoGenerateColumns="false"
					DataKeyNames="RowGuid">
					<Columns>
						<asp:TemplateField>
							<ItemTemplate>
								<%# Eval("Url") %> (<a href='mailto:<%# Eval("Email") %>'><%# Eval("Email") %></a>)
								<br />
								<%# Eval("Subject") %>
								<br />
								<%# FormatDate(Convert.ToDateTime(Eval("CreatedUtc")))%>
								<br />
								<p class="text-right">
									<asp:Button runat="server"
										ID="btnView"
										Text='<%# Resources.ContactFormResources.ContactFormViewButton %>'
										CommandArgument='<%# Eval("RowGuid") %>'
										CommandName="view"
										OnClientClick='<%# GetViewOnClick(Eval("RowGuid").ToString()) %>' SkinID="InfoButtonSmall" />
									<asp:Button runat="server"
										ID="btnDelete"
										Text='<%# Resources.ContactFormResources.ContactFormDeleteButton %>'
										CommandArgument='<%# Eval("RowGuid") %>'
										CommandName="remove"
										OnClientClick='<%# GetDeleteOnClick(Eval("RowGuid").ToString()) %>' SkinID="DeleteButtonSmall" />
								</p>
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
				</mp:mojoGridView>


				<div class="modulepager">
					<asp:HyperLink ID="lnkRefresh" runat="server" />
				</div>
			</portal:BasePanel>

			<portal:BasePanel ID="pnlCenter" runat="server" CssClass="col-sm-8" RenderId="false">
				<asp:Literal ID="litMessage" runat="server" />
				<portal:BasePanel ID="pnlMessage" runat="server" CssClass="contactmessage"></portal:BasePanel>
			</portal:BasePanel>
		</portal:BasePanel>
		<portal:mojoCutePager ID="pgrContactFormMessage" runat="server" />
	</portal:BasePanel>
</asp:Content>
