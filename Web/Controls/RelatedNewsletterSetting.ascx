<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="RelatedNewsletterSetting.ascx.cs" Inherits="mojoPortal.Web.UI.RelatedNewsletterSetting" %>

<div style="height: 250px; clear:both;">
    <asp:UpdatePanel ID="up" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <div class="floatpanel">
                <div>
                    <h3>
                        <mp:SiteLabel ID="Sitelabel4" runat="server" ConfigKey="AvailableNewsletters"
                            UseLabelTag="false" />
                    </h3>
                </div>
                <div>
                    <asp:ListBox ID="lstAll" runat="Server" Width="175" Height="175" SelectionMode="Multiple"  />
                </div>
            </div>
            <div class="floatpanel">
                <div>
                    <asp:Button Text="<-" runat="server" ID="btnRemove" CausesValidation="false" />
                    <asp:Button Text="->" runat="server" ID="btnAdd" CausesValidation="false" />
                </div>
            </div>
            <div class="floatpanel">
                <div>
                    <h3>
                        <mp:SiteLabel ID="Sitelabel5" runat="server" ConfigKey="SelectedNewsletters"
                            UseLabelTag="false" />
                    </h3>
                </div>
                <div>
                    <asp:ListBox ID="lstSelected" runat="Server" Width="175" Height="175" SelectionMode="Multiple"  />
                </div>
                <div class="clearpanel">
                    <portal:mojoLabel ID="lblMessage" runat="server" CssClass="txterror info" />
                    <asp:HiddenField ID="GuidCsv" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>