<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Admin.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.AdminPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper newsletteradmin">
<portal:HeadingControl id="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<mp:mojoGridView ID="grdLetterInfo" runat="server"
     AllowPaging="false"
     AutoGenerateColumns="false" 
     ShowHeader="false"
     CssClass="editgrid"
     DataKeyNames="LetterInfoGuid">
     <Columns>
		<asp:TemplateField>
			<ItemTemplate>
                <asp:HyperLink ID="lnkEdit" runat="server"
                 Text='<%# Eval("Title") %>' Tooltip='<%# Resources.Resource.NewsletterEditLink + " " + Eval("Title") %>'
                  NavigateUrl='<%# SiteRoot + "/eletter/LetterInfoEdit.aspx?l=" + Eval("LetterInfoGuid").ToString() %>' ></asp:HyperLink>
                
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <asp:HyperLink ID="lnkDraftList" runat="server"
                 Text='<%# Resources.Resource.NewsletterDraftListLink %>' Tooltip='<%# Resources.Resource.NewsletterDraftListLink %>'
                  NavigateUrl='<%# SiteRoot + "/eletter/LetterDrafts.aspx?l=" + Eval("LetterInfoGuid").ToString() %>' ></asp:HyperLink>
                
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <asp:HyperLink ID="lnkArchiveList" runat="server"
                 Text='<%# Resources.Resource.NewsletterArchiveListLink %>' Tooltip='<%# Resources.Resource.NewsletterArchiveListLink %>'
                  NavigateUrl='<%# SiteRoot + "/eletter/LetterArchive.aspx?l=" + Eval("LetterInfoGuid").ToString() %>' ></asp:HyperLink>
                
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <asp:HyperLink ID="lnkSubscribers" runat="server"
                 Text='<%# Eval("SubscriberCount") + " " + Resources.Resource.NewsletterSubscribersLink %>' Tooltip='<%# Resources.Resource.NewsletterSubscribersToolTip + " " + Eval("Title") %>'
                  NavigateUrl='<%# SiteRoot + "/eletter/LetterSubscribers.aspx?l=" + Eval("LetterInfoGuid").ToString() %>' ></asp:HyperLink>
                <%# string.Format(System.Globalization.CultureInfo.InvariantCulture,Resources.Resource.NewsletterUnverifiedCountFormat, Eval("UnVerifiedCount"))%>
            </ItemTemplate>
		</asp:TemplateField>
		
</Columns>
<EmptyDataTemplate>
            <p class="nodata"><asp:Literal id="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
    </EmptyDataTemplate>
</mp:mojoGridView>
    <portal:mojoCutePager ID="pgrLetterInfo" runat="server" />
   <asp:HyperLink ID="lnkAddNew" runat="server" />
    <asp:HyperLink ID="lnkManageTemplates" runat="server"></asp:HyperLink>

<portal:EmptyPanel id="divCleared1" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
