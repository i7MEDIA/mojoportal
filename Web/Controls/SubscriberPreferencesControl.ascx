<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SubscriberPreferencesControl.ascx.cs"
    Inherits="mojoPortal.Web.ELetterUI.SubscriberPreferencesControl" %>
<fieldset>
    <legend>
        <asp:Literal ID="litHeader" runat="server"></asp:Literal></legend>
    <div class="modulecontent">
        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div class="settingrow">
                    <asp:RadioButton ID="rbHtmlFormat" runat="server" GroupName="FormatPreference" />
                    <asp:RadioButton ID="rbPlainText" runat="server" GroupName="FormatPreference" />
                </div>
                <asp:Repeater ID="rptLetterPrefs" runat="server">
                    <HeaderTemplate>
                        <ul class='simplelist newsletterlist'>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li>
                            <input type="checkbox" id='chk<%# DataBinder.Eval(Container.DataItem,"LetterInfoGuid").ToString() %>'
                                <%# GetChecked(DataBinder.Eval(Container.DataItem,"LetterInfoGuid").ToString()) %>
                                title='<%# DataBinder.Eval(Container.DataItem,"Title") %>' name='chk<%# DataBinder.Eval(Container.DataItem,"LetterInfoGuid").ToString() %>' />
                            <label for='chk<%# DataBinder.Eval(Container.DataItem,"LetterInfoGuid").ToString() %>'>
                                <%# DataBinder.Eval(Container.DataItem,"Title") %></label>
                            <asp:HyperLink ID="lnkArchive" Visible='<%# Convert.ToBoolean(Eval("AllowArchiveView")) %>'
                                runat="server" Text='<%# Resources.Resource.NewsletterViewArchiveLink %>' NavigateUrl='<%# siteRoot + "/eletter/Archive.aspx?l=" + Eval("LetterInfoGuid") %>' />
                            <div class="padded">
                                <%# DataBinder.Eval(Container.DataItem,"Description") %></div>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul></FooterTemplate>
                </asp:Repeater>
                <table>
                    <tr>
                        <td>
                            <portal:mojoButton ID="btnSavePreferences" runat="server" />
                        </td>
                        <td>
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                <ProgressTemplate>
                                    <img src='<%= Page.ResolveUrl("~/Data/SiteImages/indicator1.gif") %>'
                                        alt=' ' />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
                <mp:SiteLabel ID="lblUnverifiedWarning" runat="server" CssClass="txterror info" ConfigKey="NewsletterUnverifiedWarning"
                    Visible="false"></mp:SiteLabel>
                <asp:Literal ID="litTest" runat="server"></asp:Literal>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</fieldset>
