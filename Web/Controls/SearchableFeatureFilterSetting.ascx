<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SearchableFeatureFilterSetting.ascx.cs" Inherits="mojoPortal.Web.UI.SearchableFeatureFilterSetting" %>

<div style="height: 250px; clear:both;">
    <asp:UpdatePanel ID="upFeatures" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <div class="floatpanel">
                <div>
                    <h3>
                        <mp:SiteLabel ID="Sitelabel4" runat="server" ConfigKey="SiteSettingsSiteAvailableFeaturesLabel"
                            UseLabelTag="false" />
                    </h3>
                </div>
                <div>
                    <asp:ListBox ID="lstAllFeatures" runat="Server" Width="175" Height="175" SelectionMode="Multiple"  />
                </div>
            </div>
            <div class="floatpanel">
                <div>
                    <asp:Button Text="<-" runat="server" ID="btnRemoveFeature" CausesValidation="false" />
                    <asp:Button Text="->" runat="server" ID="btnAddFeature" CausesValidation="false" />
                </div>
            </div>
            <div class="floatpanel">
                <div>
                    <h3>
                        <mp:SiteLabel ID="Sitelabel5" runat="server" ConfigKey="SiteSettingsSiteSelectedFeaturesLabel"
                            UseLabelTag="false" />
                    </h3>
                </div>
                <div>
                    <asp:ListBox ID="lstSelectedFeatures" runat="Server" Width="175" Height="175" SelectionMode="Multiple"  />
                </div>
                <div class="clearpanel">
                    <portal:mojoLabel ID="lblFeatureMessage" runat="server" CssClass="txterror info" />
                    <asp:HiddenField ID="featureGuidCsv" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
