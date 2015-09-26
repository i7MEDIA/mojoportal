<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="LetterSubscribers.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.LetterSubscribersPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkLetterAdmin" runat="server" CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper lettersubscriber">
<portal:HeadingControl id="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<asp:Panel ID="pnlSearchSubscribers" runat="server" DefaultButton="btnSearch" CssClass="settingrow subscribersearch">
<asp:TextBox ID="txtSearchInput" runat="server" CssClass="widetextbox" MaxLength="100" />
<portal:mojoButton ID="btnSearch" runat="server" />
<asp:HyperLink ID="lnkOptIn" runat="server" CssClass="cblink" Visible="false" />
</asp:Panel>
<asp:Panel ID="pnlDeleteUnVerified" runat="server" DefaultButton="btnDeleteUnVerified" CssClass="settingrow subscribersearch">
<portal:mojoButton ID="btnDeleteUnVerified" runat="server" />
<asp:TextBox ID="txtDaysOld" runat="server" Text="90" CssClass="smalltextbox" MaxLength="3" />
<mp:SiteLabel id="lblDays" runat="server" CssClass="" ConfigKey="NewslettersDays"></mp:SiteLabel>
</asp:Panel>

<mp:mojoGridView ID="grdSubscribers" runat="server" 
    AllowPaging="false" 
    AllowSorting="false"
    AutoGenerateColumns="false" 
    CssClass="" 
    DataKeyNames="Guid">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
            <asp:HyperLink Text='<%# Eval("Name") %>' id="Hyperlink2" 
						    NavigateUrl='<%# SiteRoot + "/Admin/ManageUsers.aspx?u=" + Eval("UserGuid")   %>' 
						    Visible='<%# ShowUserLink(Eval("UserGuid").ToString()) %>' runat="server" />
            <asp:Literal ID="litUser" runat="server" Text='<%# Eval("Name") %>' Visible='<%# !ShowUserLink(Eval("UserGuid").ToString()) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("Email")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("UseHtml")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.Format(Convert.ToDateTime(Eval("BeginUtc")), timeZone, "g", timeOffset) %>
                <br /> <a class='cblink' title='<%# Eval("IpAddress") %>' href='http://whois.arin.net/rest/ip/<%# Eval("IpAddress") %>.txt'><%# Eval("IpAddress")%></a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("IsVerified")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
        <ItemTemplate>
            <portal:mojoButton ID="btnSendVerificaiton" runat="server" Visible='<%# !Convert.ToBoolean(Eval("IsVerified"))%>' CommandName="SendVerification" CommandArgument='<%# Eval("Guid") %>' Text='<%$ Resources:Resource, ResendConfirmationEmailButton %>' />
            <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteSubscriber" CommandArgument='<%# Eval("Guid") %>' ToolTip='<%# Resources.Resource.NewletterSubscriberDeleteButton %>'
                AlternateText='<%# Resources.Resource.NewletterSubscriberDeleteButton %>' ImageUrl='<%# DeleteLinkImage %>' />
        </ItemTemplate>
         </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
            <p class="nodata"><asp:Literal id="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
    </EmptyDataTemplate>
</mp:mojoGridView>
<portal:mojoCutePager ID="pgrLetterSubscriber" runat="server" />
<div class="settingrow">
   <portal:mojoButton runat="server" ID="btnExport" Text="Batch Process Subscribers" />
</div>
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
