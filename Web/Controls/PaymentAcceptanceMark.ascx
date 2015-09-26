<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="PaymentAcceptanceMark.ascx.cs"
    Inherits="mojoPortal.Web.UI.PaymentAcceptanceMark" %>
<div class="floatpanel">
    <div class="floatpanel" style="padding-top: 15px;">
        <asp:Image ID="imgMasterCard" runat="server" ImageUrl="~/Data/SiteImages/mc.gif"
            AlternateText="MasterCard" Visible="false" />
        <asp:Image ID="imgVisaCard" runat="server" ImageUrl="~/Data/SiteImages/visa.gif"
            AlternateText="VISA" Visible="false" />
        <asp:Image ID="imgAmexCard" runat="server" ImageUrl="~/Data/SiteImages/amex.gif"
            AlternateText="American Express" Visible="false" />
        <asp:Image ID="imgDiscover" runat="server" ImageUrl="~/Data/SiteImages/discover.gif"
            AlternateText="Discover Card" Visible="false" />
        <table id="tblPayPal" class="paypal-accept" runat="server" visible="false" enableviewstate="false" border="0" cellpadding="10" cellspacing="0" align="center">
            <tr><td align="center"><a href="https://www.paypal.com/webapps/mpp/paypal-popup" title="How PayPal Works" onclick="javascript:window.open('https://www.paypal.com/webapps/mpp/paypal-popup','WIPaypal','toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, width=1060, height=700'); return false;"><img src="https://www.paypalobjects.com/webstatic/mktg/logo/AM_mc_vs_dc_ae.jpg" border="0" alt="PayPal Acceptance Mark"></a></td></tr></table>
    </div>

</div>
