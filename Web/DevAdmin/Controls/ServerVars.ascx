<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ServerVars.ascx.cs" Inherits="mojoPortal.Web.DevAdmin.ServerVarsControl" %>

<asp:Repeater ID="rptrServerVars" runat="server">
	<ItemTemplate>
		<div class="settingrow">
			<asp:Label ID="lblvar" runat="server" CssClass="settinglabel" Text='<%# Container.DataItem %>' />
			<asp:Literal ID="VarValue" runat="server" />
		</div>
	</ItemTemplate>
</asp:Repeater>
