<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Subscribe.ascx.cs" Inherits="mojoPortal.Web.ELetterUI.Subscribe" %>

<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
<ContentTemplate> 
<asp:Panel ID="pnlSubscribe" runat="server" DefaultButton="btnSubscribe">
<asp:Repeater ID="rptLetters" runat="server">
<HeaderTemplate><ul class='simplelist newsletterlist'></HeaderTemplate>
<ItemTemplate>
<li>
<input type="checkbox"  
id='chk<%# DataBinder.Eval(Container.DataItem,"LetterInfoGuid").ToString() %>' checked="checked"
title='<%# DataBinder.Eval(Container.DataItem,"Title") %>' name='chk<%# DataBinder.Eval(Container.DataItem,"LetterInfoGuid").ToString() %>'  /> <label for='chk<%# DataBinder.Eval(Container.DataItem,"LetterInfoGuid").ToString() %>'><%# DataBinder.Eval(Container.DataItem,"Title") %></label>
<asp:HyperLink ID="lnkArchive" Visible='<%# Convert.ToBoolean(Eval("AllowArchiveView")) && ShowPreviousEditionsLink %>' runat="server" Text='<%# Resources.Resource.NewsletterViewArchiveLink %>' NavigateUrl='<%# siteRoot + "/eletter/Archive.aspx?l=" + Eval("LetterInfoGuid") %>' />
<div id="divDescription" runat="server" visible='<%# IncludeDescriptionInList %>' class="padded"><%# DataBinder.Eval(Container.DataItem,"Description") %></div>
</li>
</ItemTemplate>
<FooterTemplate></ul></FooterTemplate>
</asp:Repeater>
<span id="spnFormat" runat="server" class="emailformat">
<asp:RadioButton ID="rbHtmlFormat" runat="server" GroupName="FormatPreference" />
<asp:RadioButton ID="rbPlainText" runat="server" GroupName="FormatPreference" /> 
</span>
<mp:WatermarkTextBox ID="txtEmail" runat="server" CssClass="watermarktextbox subscribeemail"></mp:WatermarkTextBox>
<portal:mojoButton ID="btnSubscribe" runat="server" ValidationGroup="subscribe" />
<asp:HyperLink ID="lnkMoreInfo" runat="server" />
<asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="txtEmail" ValidationGroup="subscribe" Display="None" />
<portal:EmailValidator ID="regexEmail" runat="server" ControlToValidate="txtEmail" ValidationGroup="subscribe" Display="None" />
<asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="subscribe" />
<asp:HiddenField ID="hdnJs" runat="server" Value="" />
</asp:Panel>
<asp:Panel ID="pnlThanks" runat="server" Visible="false">
<asp:Literal ID="litThankYou" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlNoNewsletters" runat="server" Visible="false">
<mp:SiteLabel id="lblWarning" runat="server" CssClass="txterror info" ConfigKey="NewslettersNotAvailable"></mp:SiteLabel>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
