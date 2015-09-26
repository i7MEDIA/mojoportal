<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="RoleUserSelectDialog.aspx.cs" Inherits="mojoPortal.Web.AdminUI.RoleUserSelectDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
<portal:ScriptLoader ID="ScriptInclude" runat="server"  />
<div  style="padding: 5px 5px 5px 5px;" class="yui-skin-sam">

<asp:Panel ID="pnlLookup" runat="server" Visible="false">
	<div class="AspNet-GridView">	
	<table  cellspacing="0" width="100%">
		<thead>
		<tr>
			<th id='<%# Resources.Resource.MemberListUserNameLabel.Replace(" ", "") %>' >
				<mp:SiteLabel id="lblUserNameLabel" runat="server" ConfigKey="MemberListUserNameLabel" UseLabelTag="false"></mp:SiteLabel>
			</th>
			<th id='<%# Resources.Resource.MemberListEmailLabel.Replace(" ", "") %>'>
				<mp:SiteLabel id="SiteLabel1" runat="server" ConfigKey="MemberListEmailLabel" UseLabelTag="false"></mp:SiteLabel>
			</th>
			<th id='<%# Resources.Resource.MemberListLoginNameLabel.Replace(" ", "") %>'>
				<mp:SiteLabel id="SiteLabel2" runat="server" ConfigKey="MemberListLoginNameLabel" UseLabelTag="false"></mp:SiteLabel>
			</th>
			<th></th>
		</tr></thead><tbody>
		<asp:Repeater id="rptUsers" runat="server" EnableViewState="False">
			<ItemTemplate>
				<tr>
					<td headers='<%# Resources.Resource.MemberListUserNameLabel.Replace(" ", "") %>'>
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString())%>
					</td>
					<td headers='<%# Resources.Resource.MemberListEmailLabel.Replace(" ", "") %>'>
					    <a href='<%# "mailto:" + Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%>'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%></a>
					</td>
					<td headers='<%# Resources.Resource.MemberListLoginNameLabel.Replace(" ", "") %>'> 
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "LoginName").ToString())%>
					</td>
					<td>
					<asp:Button ID="btnSelect" runat="server" Text='<%# Resources.Resource.UserLookupDialogSelectButton %>' CommandName="selectUser" 
                        CommandArgument='<%# Eval("UserID") %>' />
					</td>
				</tr>
			</ItemTemplate>
			<alternatingItemTemplate>
				<tr class="AspNet-GridView-Alternate">
					<td headers='<%# Resources.Resource.MemberListUserNameLabel.Replace(" ", "") %>'>
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString())%>
					</td>
					<td headers='<%# Resources.Resource.MemberListEmailLabel.Replace(" ", "") %>'>
					    <a href='<%# "mailto:" + Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%>'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%></a>
					</td>
					<td headers='<%# Resources.Resource.MemberListLoginNameLabel.Replace(" ", "") %>'> 
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "LoginName").ToString())%>
					</td>
					<td>
					<asp:Button ID="btnSelect" runat="server" Text='<%# Resources.Resource.UserLookupDialogSelectButton %>' CommandName="selectUser" 
                        CommandArgument='<%# Eval("UserID") %>' />
					</td>
				</tr>
			</AlternatingItemTemplate>
		</asp:Repeater></tbody>
	</table>
	</div>	
   <portal:mojoCutePager ID="pgrMembers" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlNotAllowed" runat="server" Visible="false">
<mp:SiteLabel ID="lblNotAllowed" runat="server" CssClass="txterror warning" UseLabelTag="false" ConfigKey="NotInUserLookupRolesWarning" />
</asp:Panel>


</div>
</asp:Content>
