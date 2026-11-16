<%@ Control language="c#" Inherits="mojoPortal.Web.XmlUI.XmlModule" CodeBehind="XmlModule.ascx.cs" AutoEventWireup="false" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">

<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper xmlmodule">
<portal:ModuleTitleControl id="Title1" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server">
            <asp:Literal id="litUnTrustedContent" runat="server" />                
 </NeatHtml:UntrustedContent>
    <asp:Literal id="litTrustedContent" runat="server" />  
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>

</portal:InnerWrapperPanel>

</portal:OuterWrapperPanel>	
