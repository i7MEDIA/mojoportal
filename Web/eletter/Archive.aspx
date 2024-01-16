<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Archive.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.ArchivePage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkNewsletters" runat="server" CssClass="unselectedcrumb" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin eletter">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<mp:mojoGridView ID="grdLetter" runat="server"
						CssClass=""
						AutoGenerateColumns="false"
						DataKeyNames="LetterGuid">
						<Columns>
							<asp:TemplateField>
								<ItemTemplate>
									<asp:HyperLink ID="lnkLetterView" runat="server" CssClass="cblink" NavigateUrl='<%# SiteRoot + "/eletter/LetterView.aspx?l=" + Eval("LetterInfoGuid") + "&letter=" + Eval("LetterGuid") %>' Text='<%# Eval("Subject") %>' ToolTip='<%# Eval("Subject") %>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField>
								<ItemTemplate>
									<%# DateTimeHelper.Format(Convert.ToDateTime(Eval("SendClickedUTC")), timeZone, "g", timeOffset) %>
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
						<EmptyDataTemplate>
							<p class="nodata">
								<asp:Literal ID="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" />
							</p>
						</EmptyDataTemplate>
					</mp:mojoGridView>
					<portal:mojoCutePager ID="pgrLetter" runat="server" />
					<div class="settingrow">
						<asp:Label ID="lblMessage" runat="server" />
					</div>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
