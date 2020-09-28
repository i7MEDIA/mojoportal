<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="IndexBrowser.aspx.cs" Inherits="mojoPortal.Web.AdminUI.IndexBrowser" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<style type="text/css">
		.label-v-center { 
			vertical-align: middle; 
			margin: 0; 
		} 
 
		button.ui-datepicker-trigger { 
			padding: 1px 10px 6px 10px !important;
		    font-weight: 700 !important;
		    margin: 0 !important;
		    border: 1px solid #eee !important;
		    background-color: #f7f7f7 !important;
		} 
 
		.label-width-small { 
			width: auto; 
		} 
 
		.input-group-separated > * { 
			margin: 0 10px; 
			flex: 1 1 auto; 
		} 
 
		.input-group-separated { 
			display: flex; 
			flex-flow: row wrap; 
			justify-content: center; 
			align-items: center; 
		} 
 
		.input-as-text { 
			border: 1px solid #eee !important;
		    margin: 0;
		    font-weight: 700;
		    width: auto !important;
		    float: none !important;
		    box-shadow: none !important;
		    border-right: 0 none !important;
		}

		.padded-group {
			padding: 5px 0;
		}
	</style>
	<script type="text/javascript">
		$(document).ready(function () {
			$('.date-picker').addClass('input-as-text');
		});
	</script>
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

					<div class="row">
						<div class="col-md-2">
							<div class="panel panel-primary">
								<div class="panel-heading">
									<h3 class="panel-title"><%= Resources.Resource.AdminIndexBrowserFilterContentType %></h3>
								</div>
								<div class="panel-body">
									<div class="form-inline">
										<div class="form-group">
											<div class="input-group">
												<asp:DropDownList ID="ddFeatureList" runat="server" CssClass="searchfeatures"></asp:DropDownList>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
						<div class="col-md-6">
							<div class="panel panel-primary">
								<div class="panel-heading">
									<h3 class="panel-title"><%= Resources.Resource.AdminIndexBrowserFilterDate %></h3>
								</div>
								<div class="panel-body">
									<div class="form-inline">
										<div class="form-group padded-group">
											<div class="input-group input-group-sm input-group-separated">
												<mp:SiteLabel runat="server" EnableViewState="false" ConfigKey="AdminIndexBrowserModifiedBetween" CssClass="label-v-center settinglabel label-width-small" />
												<portal:jDatePicker ID="beginDate" runat="server" />
												<mp:SiteLabel runat="server" EnableViewState="false" ConfigKey="and" CssClass="label-v-center settinglabel label-width-small" />
												<portal:jDatePicker ID="endDate" runat="server" />
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
						<div class="col-md-4">
							<div class="panel panel-primary">
								<div class="panel-heading">
									<h3 class="panel-title"><%= Resources.Resource.AdminIndexBrowserActionsHeading %></h3>
								</div>
								<div class="panel-body">
									<div class="form-inline">
										<div class="form-group">
											<div class="input-group input-group-separated input-group-btn">
												<portal:mojoButton ID="btnGo" runat="server" SkinID="SuccessButton" />
												<a href='<%= SiteRoot %>/Admin/IndexBrowser.aspx' class="btn btn-warning"><%= Resources.Resource.AdminIndexBrowserClearFilter %></a>
												<portal:mojoButton ID="btnRebuildSearchIndex" runat="server" SkinID="DangerButton" />
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
					<div>
						<portal:mojoLabel ID="lblMessage" runat="server" SkinID="error" />
					</div>


					<asp:Panel ID="pnlSearchResults" runat="server" CssClass="settingrow searchresults">
						<portal:mojoCutePager ID="pgrTop" runat="server" Visible="false" />
						<asp:Repeater ID="rptResults" runat="server" EnableViewState="true" OnItemCommand="rptResults_ItemCommand" OnItemDataBound="rptResults_ItemDataBound">
							<ItemTemplate>
								<div class="result">
									<h3>
										<asp:HyperLink ID="Hyperlink1" runat="server" EnableViewState="false"
											NavigateUrl='<%# BuildUrl((mojoPortal.SearchIndex.IndexItem)Container.DataItem) %>'
											Text='<%# FormatItemTitle(Eval("PageName").ToString(), Eval("ModuleTitle").ToString(), Eval("Title").ToString(), ">")  %>' />
									</h3>
									<div class="row">
										<div class="col-md-3">
											<div class="well">
												<h4><%# Resources.Resource.AdminIndexBrowserItemDetails %></h4>
												<strong><%# Resources.Resource.AdminIndexBrowserDocKey %>:</strong> <%# Eval("DocKey").ToString() %><br />
												<strong><%# Resources.Resource.AdminIndexBrowserItemUrl %>:</strong> <%# Eval("ViewPage").ToString() %><br />
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
