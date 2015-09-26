<%@ Page Language="c#" ValidateRequest="false" MaintainScrollPositionOnPostback="true"
    EnableViewStateMac="false" CodeBehind="SearchResults.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="mojoPortal.Web.UI.Pages.SearchResults" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <mp:CornerRounderTop ID="ctop1" runat="server" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper searchresults">
            <portal:HeadingControl ID="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                    <portal:SearchResultsDisplaySettings id="displaySettings" runat="server" />
                    <asp:Panel ID="pnlInternalSearch" runat="server" DefaultButton="btnDoSearch">
                        <div id="divDelete" runat="server" visible="false" class="settingrow">
                            <portal:mojoButton ID="btnRebuildSearchIndex" runat="server" />
                        </div>
                        <div class="settingrow searchcontrols">
                            <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="search-help" />
                            <asp:TextBox ID="txtSearchInput" runat="server" Columns="50" MaxLength="255" CssClass="widetextbox searchbox"></asp:TextBox>
                            <asp:DropDownList ID="ddFeatureList" runat="server" CssClass="searchfeatures">
                            </asp:DropDownList>
                            <span class="s-datefilters" id="spnDateRange" runat="server">
                            <br class="s-datefilterbreak" /><asp:Literal ID="litDatePreamble" runat="server" EnableViewState="false" /> <mp:DatePickerControl ID="dpBeginDate" runat="server" SkinID="search"  CssClass="forminput" />
                            <asp:Literal ID="litAnd" runat="server" EnableViewState="false" /> <mp:DatePickerControl ID="dpEndDate" runat="server" SkinID="search"  CssClass="forminput" /></span>
                            <portal:mojoButton ID="btnDoSearch" runat="server" CausesValidation="false" UseSubmitBehavior="true" />
                            <span class="searchduration">
                                <asp:Label ID="lblDuration" runat="server" Visible="False"></asp:Label>
                                <mp:SiteLabel ID="lblSeconds" runat="server" Visible="False" ConfigKey="SearchResultsSecondsLabel"
                                    UseLabelTag="false"></mp:SiteLabel>
                            </span>
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </div>
                        <div id="divResults" runat="server" class="settingrow searchresultsummary">
                            <mp:SiteLabel ID="lblReslts" runat="server" ConfigKey="SearchResultsLabel" UseLabelTag="false">
                            </mp:SiteLabel>
                            <asp:Label ID="lblFrom" runat="server" Font-Bold="True"></asp:Label>-<asp:Label ID="lblTo"
                                runat="server" Font-Bold="True"></asp:Label>
                            <mp:SiteLabel ID="Sitelabel1" runat="server" ConfigKey="SearchResultsOfLabel" UseLabelTag="false">
                            </mp:SiteLabel>
                            <asp:Label ID="lblTotal" runat="server" Font-Bold="True"></asp:Label>
                            <mp:SiteLabel ID="lblFor" runat="server" ConfigKey="SearchResultsForLabel" UseLabelTag="false">
                            </mp:SiteLabel>
                            <asp:Label ID="lblQueryText" runat="server" Font-Bold="True" CssClass="searchqueryterm"></asp:Label>
                        </div>
                        <asp:Panel ID="pnlSearchResults" runat="server" Visible="False" CssClass="settingrow searchresults">
                            <portal:mojoCutePager ID="pgrTop" runat="server" Visible="false" />
                            <asp:Repeater ID="rptResults" runat="server" EnableViewState="False">
                                <HeaderTemplate>
                                    <ol class="searchresultlist">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li class="searchresult">
                                        <NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" TrustedImageUrlPattern='<%# mojoPortal.Web.Framework.SecurityHelper.RegexRelativeImageUrlPatern %>'
                                            ClientScriptUrl="~/ClientScript/NeatHtml.js">
                                            <<%# displaySettings.ItemHeadingElement %>>
                                                <asp:HyperLink ID="Hyperlink1" runat="server" 
                                                    NavigateUrl='<%# BuildUrl((mojoPortal.SearchIndex.IndexItem)Container.DataItem) %>'
                                                    Text='<%# FormatLinkText(Eval("PageName").ToString(), Eval("ModuleTitle").ToString(), Eval("Title").ToString())  %>' />
                                                </<%# displaySettings.ItemHeadingElement %>>
		                                        
                                                <div class="searchresultdesc">
                                                    <%# Eval("Intro").ToString() %>
                                                </div>
                                            <%# FormatAuthor(Eval("Author").ToString()) %>
                                            <%# FormatCreatedDate((mojoPortal.SearchIndex.IndexItem)Container.DataItem) %>
                                            <%# FormatModifiedDate((mojoPortal.SearchIndex.IndexItem)Container.DataItem) %>
                                        </NeatHtml:UntrustedContent>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ol>
                                </FooterTemplate>
                            </asp:Repeater>
                            <div>
                                &nbsp;</div>
                            <portal:mojoCutePager ID="pgrBottom" runat="server" Visible="false" />
                        </asp:Panel>
                        <asp:Panel ID="pnlNoResults" runat="server" Visible="False">
                            <asp:Label ID="lblNoResults" runat="server"></asp:Label>
                        </asp:Panel>
                        <div class="settingrow">
                            <span id="spnAltSearchLinks" runat="server" visible="false">
                                <asp:Literal ID="litAltSearchMessage" runat="server" />
                                <asp:HyperLink ID="lnkBingSearch" runat="server" Visible="false" CssClass="extrasearchlink" />
                                <asp:HyperLink ID="lnkGoogleSearch" runat="server" Visible="false" CssClass="extrasearchlink" />
                            </span>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlGoogleSearch" runat="server" Visible="false" CssClass="gcswrap">
                        <portal:GoogleCustomSearchControl ID="gcs" runat="server" Visible="false" />
                    </asp:Panel>
                    <asp:Panel ID="pnlBingSearch" runat="server" Visible="false" CssClass="searchresults bingresults">
                        <portal:BingSearchControl ID="bingSearch" runat="server" Visible="false" />
                    </asp:Panel>
                </portal:InnerBodyPanel>
            </portal:OuterBodyPanel>
            <portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared">
            </portal:EmptyPanel>
        </portal:InnerWrapperPanel>
        <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
