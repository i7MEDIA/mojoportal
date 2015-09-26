<%@ Page CodeBehind="EditAccessDenied.aspx.cs" Language="c#" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="mojoPortal.Web.UI.Pages.EditAccessDenied" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="modulecontent accessdenied">
       
            <mp:SiteLabel ID="lblEditAccessDeniedLabel" runat="server" ConfigKey="EditAccessDeniedLabel"
                CssClass="txterror warning" UseLabelTag="false"></mp:SiteLabel>
        
        <p>
            <asp:HyperLink ID="lnkHome" runat="server" />
        </p>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
