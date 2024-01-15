<%@ Page Language="c#" CodeBehind="XmlEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
	AutoEventWireup="false" Inherits="mojoPortal.Web.XmlUI.EditXml" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper xmlmodule">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Panel ID="pnlEdit" runat="server" DefaultButton="updateButton">
						<div class="settingrow">
							<mp:SiteLabel ID="lblXmlDataFile" runat="server" ForControl="ddXml" CssClass="settinglabel" ConfigKey="EditXmlFileLabel" ResourceFile="XmlResources" />
							<asp:DropDownList ID="ddXml" runat="server" EnableTheming="false" CssClass="forminput" DataValueField="Name" DataTextField="Name" />
						</div>
						<div class="settingrow">
							<mp:SiteLabel ID="lblXslFile" runat="server" ForControl="ddXsl" CssClass="settinglabel" ConfigKey="EditXslFileLabel" ResourceFile="XmlResources" />
							<asp:DropDownList ID="ddXsl" runat="server" EnableTheming="false" CssClass="forminput" DataValueField="Name" DataTextField="Name" />
						</div>
						<div class="settingrow">
							<mp:SiteLabel ID="SiteLabel2" runat="server" ForControl="txtXmlUrl" CssClass="settinglabel" ConfigKey="XmlUrl" ResourceFile="XmlResources" />
							<asp:TextBox ID="txtXmlUrl" runat="server" CssClass="verywidetextbox forminput" />
						</div>
						<div class="settingrow">
							<mp:SiteLabel ID="SiteLabel4" runat="server" ForControl="txtXslUrl" CssClass="settinglabel" ConfigKey="XslUrl" ResourceFile="XmlResources" />
							<asp:TextBox ID="txtXslUrl" runat="server" CssClass="verywidetextbox forminput" />
						</div>
						<div class="settingrow">
							<mp:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
							<div class="forminput">
								<portal:mojoButton ID="updateButton" runat="server" />&nbsp;
								<asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;
							</div>
							<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="xml-edit-help" />
						</div>
						<div class="settingrow">
							<portal:jQueryFileUpload ID="uploader" runat="server" />
							<asp:HiddenField ID="hdnState" Value="images" runat="server" />
						</div>
						<div class="settingrow">
							<mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
							<div class="forminput">
								<portal:mojoButton ID="btnUpload" runat="server" />&nbsp;&nbsp;
								<asp:RegularExpressionValidator ID="regexFile" ControlToValidate="uploader" ValidationExpression="(([^.;]*[.])+(xml|xsl|XML|XSL); *)*(([^.;]*[.])+(xml|xsl|XML|XSL))?$" Display="Static" EnableClientScript="true" runat="server" />
							</div>
						</div>
					</asp:Panel>
					<asp:HiddenField ID="hdnReturnUrl" runat="server" />
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			
		</portal:InnerWrapperPanel>

	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
