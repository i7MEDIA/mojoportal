<%@ Page Language="c#" ValidateRequest="false" MaintainScrollPositionOnPostback="true"
	EnableViewStateMac="false" CodeBehind="SearchResults.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
	AutoEventWireup="false" Inherits="mojoPortal.Web.UI.Pages.SearchResults" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper searchresults">
			<portal:HeadingControl ID="heading" runat="server" />

			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<portal:SearchResultsDisplaySettings ID="displaySettings" runat="server" />

					<asp:Panel ID="pnlInternalSearch" runat="server">
						<asp:Literal runat="server" ID="litSearchResults" />

						<div id="spnAltSearchLinks" runat="server" visible="false" class="settingrow">
							<asp:Literal ID="litAltSearchMessage" runat="server" />
							<asp:HyperLink ID="lnkBingSearch" runat="server" Visible="false" CssClass="extrasearchlink" />
							<asp:HyperLink ID="lnkGoogleSearch" runat="server" Visible="false" CssClass="extrasearchlink" />
						</div>
					</asp:Panel>

					<asp:Panel ID="pnlGoogleSearch" runat="server" Visible="false" CssClass="gcswrap">
						<portal:GoogleCustomSearchControl ID="gcs" runat="server" Visible="false" />
					</asp:Panel>

					<asp:Panel ID="pnlBingSearch" runat="server" Visible="false" CssClass="searchresults bingresults">
						<portal:BingSearchControl ID="bingSearch" runat="server" Visible="false" />
					</asp:Panel>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>

<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
