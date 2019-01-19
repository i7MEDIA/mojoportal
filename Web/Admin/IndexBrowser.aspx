<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="IndexBrowser.aspx.cs" Inherits="mojoPortal.Web.AdminUI.IndexBrowser" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />
		<portal:AdminCrumbSeparator ID="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkAdvancedTools" runat="server" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin admin-index-browser">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">

					<div>
						<asp:DropDownList ID="ddFeatureList" runat="server" CssClass="searchfeatures"></asp:DropDownList>
						<mp:SiteLabel runat="server" EnableViewState="false" ConfigKey="AdminIndexBrowserModifiedBetween" />
						<portal:jDatePicker ID="beginDate" runat="server" />
						<mp:SiteLabel runat="server" EnableViewState="false" ConfigKey="and" />

						<portal:jDatePicker ID="endDate" runat="server" />
						<asp:Button ID="btnGo" runat="server" Text="GO" OnClick="btnGo_Click" />
						<a href='<%= SiteRoot %>/Admin/IndexBrowser.aspx'><%= Resources.Resource.AdminIndexBrowserClearFilter %></a>
					</div>
					<div>
						<portal:mojoButton ID="btnRebuildSearchIndex" runat="server" OnClick="btnRebuildSearchIndex_Click" />

					</div>
					<div>
						<portal:mojoLabel ID="lblMessage" runat="server" />
					</div>


					<asp:Panel ID="pnlSearchResults" runat="server" CssClass="settingrow searchresults">
						<portal:mojoCutePager ID="pgrTop" runat="server" Visible="false" />
						<asp:Repeater ID="rptResults" runat="server" EnableViewState="true" OnItemCommand="rptResults_ItemCommand">
							<ItemTemplate>
								<div class="result">
									<h3>
										<asp:HyperLink ID="Hyperlink1" runat="server" EnableViewState="false"
											NavigateUrl='<%# BuildUrl((mojoPortal.SearchIndex.IndexItem)Container.DataItem) %>'
											Text='<%# FormatLinkText(Eval("PageName").ToString(), Eval("ModuleTitle").ToString(), Eval("Title").ToString())  %>' />
									</h3>
									<div class="row">
										<div class="col-md-3">
											<div class="well">
												<h4><%# Resources.Resource.AdminIndexBrowserItemDetails %></h4>
												<strong><%# Resources.Resource.AdminIndexBrowserDocKey %>:</strong> <%# Eval("DocKey").ToString() %><br />
												<strong><%# Resources.Resource.AdminIndexBrowserAuthor %>:</strong> <%# FormatProperty(Eval("Author").ToString()) %><br />
												<strong><%# Resources.Resource.AdminIndexBrowserPageViewRoles %>:</strong> <%# FormatProperty(Eval("ViewRoles").ToString()) %><br />
												<strong><%# Resources.Resource.AdminIndexBrowserModuleViewRoles %>:</strong> <%# FormatProperty(Eval("ModuleViewRoles").ToString()) %><br />
												<strong><%# Resources.Resource.AdminIndexBrowserPageMetaDesc %>:</strong> <%# FormatProperty(Eval("PageMetaDescription").ToString()) %><br />
												<strong><%# Resources.Resource.AdminIndexBrowserPageMetaKeywords %>:</strong> <%# FormatProperty(Eval("PageMetaKeywords").ToString()) %><br />
												<strong><%# Resources.Resource.AdminIndexBrowserCreatedDate %>:</strong> <%# FormatProperty(Eval("CreatedUtc").ToString()) %><br />
												<strong><%# Resources.Resource.AdminIndexBrowserModifiedDate %>:</strong> <%# FormatProperty(Eval("LastModUtc").ToString()) %><br />
												<portal:mojoButton ID="btnDelete" runat="server" Text="Delete" CommandName="delete" CommandArgument='<%# Eval("DocKey").ToString() %>' SkinID="DeleteButton" />
											</div>
										</div>
										<div class="col-md-8">
											<h4><%# Resources.Resource.AdminIndexBrowserItemIntro %></h4>
											<asp:Literal ID="litIntro" runat="server" EnableViewState="false" Text='<%# Eval("ContentAbstract").ToString() %>' />
										</div>
									</div>
								</div>
							</ItemTemplate>
						</asp:Repeater>
						<portal:mojoCutePager ID="pgrBottom" runat="server" Visible="false" />
					</asp:Panel>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
