<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SurveyPageEdit.aspx.cs" Inherits="SurveyFeature.UI.PageEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<div class="breadcrumbs">
		<asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb" /> &gt;
	    <asp:HyperLink runat="server" ID="lnkSurveys" CssClass="unselectedcrumb" /> &gt;
		<asp:HyperLink runat="server" ID="lnkPages" CssClass="unselectedcrumb" /> &gt;
		<asp:HyperLink runat="server" ID="lnkPageEdit" CssClass="selectedcrumb" />
	</div>

	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper survey">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Panel ID="pnlPageEdit" runat="server" DefaultButton="btnSave">
						<portal:FormGroupPanel runat="server">
							<mp:SiteLabel runat="server"
								ID="lblPageTitleLabel"
								ConfigKey="PageTitleLabel"
								ResourceFile="SurveyResources"
								CssClass="settinglabel" />
							<asp:TextBox runat="server" ID="txtPageTitle" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server">
							<mp:SiteLabel runat="server"
								ID="lblPageEnabledLabel"
								ConfigKey="PageEnabledLabel"
								ResourceFile="SurveyResources"
								CssClass="settinglabel" />
							<asp:CheckBox runat="server" ID="chkPageEnabled" Checked="true" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server" ExtraCssClasses="btn-row">
							<portal:mojoButton ID="btnSave" runat="server" SkinID="SaveButton" />
							<portal:mojoButton ID="btnCancel" runat="server" SkinID="TextButton" />
						</portal:FormGroupPanel>
					</asp:Panel>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared" />
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
	<portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
