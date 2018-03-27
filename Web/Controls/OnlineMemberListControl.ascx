<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OnlineMemberListControl.ascx.cs" Inherits="mojoPortal.Web.UI.OnlineMemberListControl" %>

<asp:Repeater ID="rptOnlineMembers" runat="server" EnableViewState="false">
 <HeaderTemplate>
	<h3>
		<mp:SiteLabel id="slMembersOnline" runat="server"  ConfigKey="OnlineMembersLabel" UseLabelTag="false" />
	</h3>
	<ul>
 </HeaderTemplate>
 <ItemTemplate>
 <li>
	 <portal:Avatar id="av1" runat="server" SkinID="OnlineMembers"
		UseLink='true'
		MaxAllowedRating='<%# MaxAllowedGravatarRating %>'
		AvatarFile='<%# Eval("AvatarUrl") %>'
		UserName='<%# Eval("Name") %>'
		UserId='<%# Convert.ToInt32(Eval("UserID")) %>'
		SiteId='<%# siteSettings.SiteId %>'
		SiteRoot='<%# SiteRoot %>'
		Email='<%# Eval("Email") %>'
		Disable='<%# disableAvatars %>'
		UseGravatar='<%# allowGravatars %>'
		Size="60"
		/>
	 <div>
		 <%# SiteUtils.GetProfileLink(Page, DataBinder.Eval(Container.DataItem,"UserID"),DataBinder.Eval(Container.DataItem,"Name")) %>
		 <asp:HyperLink Text="Edit User" ID="Hyperlink2" EnableViewState="false"
			 NavigateUrl='<%# SiteRoot + "/Admin/ManageUsers.aspx" + "?userid=" + DataBinder.Eval(Container.DataItem,"UserID") %>'
			 Visible="<%# IsAdmin %>" runat="server" CssClass="text-small"/>
	 </div>
 </li>
 </ItemTemplate>
 <FooterTemplate>
 </ul>
 </FooterTemplate>
</asp:Repeater>
