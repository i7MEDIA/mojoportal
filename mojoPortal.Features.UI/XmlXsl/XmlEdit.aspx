<%@ Page Language="c#" CodeBehind="XmlEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="mojoPortal.Web.XmlUI.EditXml" %>

<asp:content contentplaceholderid="leftContent" id="MPLeftPane" runat="server" />
<asp:content contentplaceholderid="mainContent" id="MPContent" runat="server">
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper xmlmodule">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <asp:panel id="pnlEdit" runat="server" defaultbutton="updateButton">
        <div class="settingrow">
            <mp:SiteLabel ID="lblXmlDataFile" runat="server" ForControl="ddXml" CssClass="settinglabel"
                ConfigKey="EditXmlFileLabel" ResourceFile="XmlResources"></mp:SiteLabel>
            <asp:dropdownlist id="ddXml" runat="server" enabletheming="false" cssclass="forminput"
                datavaluefield="Name" datatextfield="Name">
            </asp:dropdownlist>
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="lblXslFile" runat="server" ForControl="ddXsl" CssClass="settinglabel"
                ConfigKey="EditXslFileLabel" ResourceFile="XmlResources"></mp:SiteLabel>
            <asp:dropdownlist id="ddXsl" runat="server" enabletheming="false" cssclass="forminput"
                datavaluefield="Name" datatextfield="Name">
            </asp:dropdownlist>
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="SiteLabel2" runat="server" ForControl="txtXmlUrl" CssClass="settinglabel"
                ConfigKey="XmlUrl" ResourceFile="XmlResources"></mp:SiteLabel>
            <asp:TextBox id="txtXmlUrl" runat="server" CssClass="verywidetextbox forminput" />
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="SiteLabel4" runat="server" ForControl="txtXslUrl" CssClass="settinglabel"
                ConfigKey="XslUrl" ResourceFile="XmlResources"></mp:SiteLabel>
            <asp:TextBox id="txtXslUrl" runat="server" CssClass="verywidetextbox forminput" />
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
            <div class="forminput">
                <portal:mojoButton ID="updateButton" runat="server" />&nbsp;
                <asp:hyperlink id="lnkCancel" runat="server" cssclass="cancellink" />&nbsp;
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
                <asp:regularexpressionvalidator id="regexFile" controltovalidate="uploader" validationexpression="(([^.;]*[.])+(xml|xsl|XML|XSL); *)*(([^.;]*[.])+(xml|xsl|XML|XSL))?$"
                    display="Static" enableclientscript="true" runat="server" />
            </div>
        </div>
    </asp:panel>   
    <asp:hiddenfield id="hdnReturnUrl" runat="server" />
    </portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:content>
<asp:content contentplaceholderid="rightContent" id="MPRightPane" runat="server" />
<asp:content contentplaceholderid="pageEditContent" id="MPPageEdit" runat="server" />