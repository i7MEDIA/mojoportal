<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ServerVars.ascx.cs" Inherits="mojoPortal.Web.DevAdmin.ServerVarsControl" %>

<asp:Repeater ID="rptrServerVars" Runat="server">
	<ItemTemplate>
	<div class="settingrow">
	    <asp:Label ID="lblvar" runat="server" CssClass="settinglabel" Text='<%# Container.DataItem %>' />
	    <asp:Literal ID="VarValue" Runat="server" />
	 </div>
	</ItemTemplate>
</asp:Repeater>
