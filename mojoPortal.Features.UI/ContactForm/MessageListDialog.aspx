<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master"CodeBehind="MessageListDialog.aspx.cs" Inherits="mojoPortal.Web.ContactUI.MessageListDialog" %>
<%@ Register Namespace="mojoPortal.Web.ContactUI" Assembly="mojoPortal.Features.UI" TagPrefix="contact" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
	<contact:ContactFormDisplaySettings runat="server" ID="displaySettings" />

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
		<portal:BasePanel runat="server" ID="rowPnl" RenderId="false">
			<portal:BasePanel ID="pnlLeft" runat="server" CssClass="col-sm-4" RenderId="false">
				<mp:mojoGridView runat="server"
					ID="grdContactFormMessage"
					AllowPaging="false"
					AllowSorting="false"
					CssClass=""
					TableCssClass="jqtable"
					AutoGenerateColumns="false"
					DataKeyNames="RowGuid"
				>
					<Columns>
						<asp:TemplateField>
							<ItemTemplate>
								<%# Eval("Url") %>
								<br />
								<a href='mailto:<%# Eval("Email") %>'>
									<%# Eval("Email") %>
								</a>
								<br />
								<%# Eval("Subject") %>
								<br />
								<%# FormatDate(Convert.ToDateTime(Eval("CreatedUtc")))%>
								<br />
								<asp:Button runat="server"
									ID="btnView"
									Text='<%# Resources.ContactFormResources.ContactFormViewButton %>'
									CommandArgument='<%# Eval("RowGuid") %>'
									CommandName="view"
									OnClientClick='<%# GetViewOnClick(Eval("RowGuid").ToString()) %>'
								/>
								<asp:Button runat="server"
									ID="btnDelete"
									Text='<%# Resources.ContactFormResources.ContactFormDeleteButton %>'
									CommandArgument='<%# Eval("RowGuid") %>'
									CommandName="remove"
									OnClientClick='<%# GetDeleteOnClick(Eval("RowGuid").ToString()) %>'
								/>
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
				</mp:mojoGridView>

				<portal:mojoCutePager ID="pgrContactFormMessage" runat="server" />

				<div class="modulepager">
					<asp:HyperLink ID="lnkRefresh" runat="server" />
				</div>
			</portal:BasePanel>

			<portal:BasePanel ID="pnlCenter" runat="server" CssClass="col-sm-8" RenderId="false">
				<asp:Literal ID="litMessage" runat="server" />
				<portal:BasePanel ID="pnlMessage" runat="server" CssClass="contactmessage"></portal:BasePanel>
			</portal:BasePanel>
		</portal:BasePanel>
	</portal:BasePanel>
</asp:Content>