<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="PageTree.aspx.cs" Inherits="mojoPortal.Web.AdminUI.PageTreePage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="divAdminLinks" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkPageTree" runat="server" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop ID="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin pagetree">
<portal:HeadingControl id="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<div class="settingrow">
    <a id="lnkNewPage" runat="server" ></a>&nbsp;<portal:mojoLabel ID="litWarning" runat="server" CssClass="txterror warning" />
</div>
<div class="settingrow">
    <table cellpadding="0" cellspacing="0" border="0">
		<tr valign="top">
		    <td>
                
            </td>
			<td>
				<asp:ListBox id="lbPages" SkinID="PageTree"  DataTextField="PageName" DataValueField="PageID" Rows="30"  runat="server" />
			</td>
			
			<td class="ptreebuttons">
                <asp:ImageButton id="btnTop" CommandName="top"  runat="server" />
                <br />
				<asp:ImageButton id="btnUp" CommandName="up"  runat="server" />
				<br />
				<asp:ImageButton id="btnDown" CommandName="down" runat="server" />
                <br />
                <asp:ImageButton id="btnBottom" CommandName="up"  runat="server" />
			    <br />
				<asp:ImageButton id="btnSettings" runat="server" />
					<asp:ImageButton id="btnEdit" runat="server" />
					<asp:ImageButton ID="btnViewPage" runat="server" />
                <br />
					<asp:ImageButton id="btnSortChildPagesAlpha" runat="server" />	
				<br /><br />
					<asp:ImageButton id="btnDelete" runat="server" />	
				<br /><br />
				<portal:mojoHelpLink ID="MojoHelpLink4" runat="server" HelpKey="addeditpageshelp" />
			</td>
		</tr>
	</table>
</div>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom ID="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
