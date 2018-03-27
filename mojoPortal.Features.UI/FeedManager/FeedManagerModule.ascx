<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="FeedManagerModule.ascx.cs" Inherits="mojoPortal.Web.FeedUI.FeedManagerModule" %>
<%@ Register TagPrefix="NeatHtml" Namespace="Brettle.Web.NeatHtml" Assembly="Brettle.Web.NeatHtml" %>
<%@ Register Namespace="mojoPortal.Web.FeedUI" Assembly="mojoPortal.Features.UI" TagPrefix="feed" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper rssfeedmodule">
        <portal:ModuleTitleControl ID="Title1" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
            <feed:FeedManagerDisplaySettings ID="displaySettings" runat="server" />
            <asp:UpdatePanel ID="updPnlRSSA" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent rsswrapper">
                        <asp:Panel ID="divNav" runat="server" CssClass="rssnavright" SkinID="plain">
                            <asp:Label ID="lblFeedListName" Font-Bold="True" runat="server"></asp:Label>
                            <a id="lnkAggregateRSS" href="~/FeedManager/FeedAggregate.aspx" runat="server" class="feedlink feedag" enableviewstate="false">
                                <img alt="RSS" id="imgAggregateRSS" runat="server" enableviewstate="false" /></a>
                            <portal:mojoDataList ID="dlstFeedList" runat="server" EnableViewState="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="editLink" runat="server" CssClass="editlink" EnableViewState="false" Text="<%# Resources.FeedResources.EditImageAltText%>"
                                        ToolTip="<%# Resources.FeedResources.EditImageAltText%>" ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
                                        NavigateUrl='<%# this.SiteRoot + "/FeedManager/FeedEdit.aspx?pageid=" + PageId.ToString() + "&amp;ItemID=" + Eval("ItemID") + "&amp;mid=" + ModuleId.ToString()  %>'
                                        Visible="<%# IsEditable %>" />
                                    <asp:HyperLink ID="Hyperlink2" runat="server" CssClass='<%# "feedlink lnk" + Eval("ItemID")%>' EnableViewState="false" Visible="<%# LinkToAuthorSite && (!displaySettings.UseNoFollowOnFeedSiteLinks) %>" 
                                        NavigateUrl='<%# Eval("Url")%>'><%# DataBinder.Eval(Container, "DataItem.Author")%>
                                    </asp:HyperLink>
                                    <portal:NoFollowHyperlink ID="Hyperlink4" runat="server" CssClass='<%# "feedlink lnk" + Eval("ItemID")%>' EnableViewState="false" Visible="<%# LinkToAuthorSite && displaySettings.UseNoFollowOnFeedSiteLinks %>" 
                                        NavigateUrl='<%# Eval("Url")%>'><%# DataBinder.Eval(Container, "DataItem.Author")%>
                                    </portal:NoFollowHyperlink>
                                    <asp:Button runat="server" Visible="<%# config.UseFeedListAsFilter %>"
                                        CommandName="filter" CommandArgument='<%# Eval("ItemID")%>' Text='<%# Eval("Author")%>' CssClass="buttonlink" />
                                    <portal:NoFollowHyperlink ID="Hyperlink3" runat="server" CssClass='<%# "feed" + Eval("ItemID")%>' EnableViewState="false" Visible="<%# config.ShowIndividualFeedLinks %>"
                                        ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + RssImageFile %>' NavigateUrl='<%# Eval("RssUrl")%>'>
                                    </portal:NoFollowHyperlink>&nbsp;&nbsp;
                                </ItemTemplate>
                            </portal:mojoDataList>
                            <asp:Repeater ID="rptFeedListTop" runat="server" >
                                <HeaderTemplate><ul class="simplelist feedlist"></HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <asp:HyperLink ID="lnkEdit" runat="server" CssClass="editlink" EnableViewState="false" Text="<%# Resources.FeedResources.EditImageAltText%>"
                                        ToolTip="<%# Resources.FeedResources.EditImageAltText%>" ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
                                        NavigateUrl='<%# this.SiteRoot + "/FeedManager/FeedEdit.aspx?pageid=" + PageId.ToString() + "&amp;ItemID=" + Eval("ItemID") + "&amp;mid=" + ModuleId.ToString()  %>'
                                        Visible="<%# IsEditable %>" />
                                    <asp:HyperLink ID="lnkItem" runat="server" CssClass='<%# "feedsitelink lnk" + Eval("ItemID")%>' EnableViewState="false" Visible="<%# LinkToAuthorSite && (!displaySettings.UseNoFollowOnFeedSiteLinks) %>" 
                                        NavigateUrl='<%# Eval("Url")%>'><%# DataBinder.Eval(Container, "DataItem.Author")%>
                                    </asp:HyperLink>
                                    <portal:NoFollowHyperlink ID="HyperLink7" runat="server" CssClass='<%# "feedsitelink lnk" + Eval("ItemID")%>' EnableViewState="false" Visible="<%# LinkToAuthorSite && displaySettings.UseNoFollowOnFeedSiteLinks %>" 
                                        NavigateUrl='<%# Eval("Url")%>'><%# DataBinder.Eval(Container, "DataItem.Author")%>
                                    </portal:NoFollowHyperlink>
                                    <asp:Button ID="btnFilter" runat="server" Visible="<%# config.UseFeedListAsFilter %>"
                                        CommandName="filter" CommandArgument='<%# Eval("ItemID")%>' Text='<%# Eval("Author")%>' CssClass="buttonlink" />
                                    <portal:NoFollowHyperlink ID="lnkFeed" runat="server" CssClass='<%# "feedlink feed" + Eval("ItemID")%>' EnableViewState="false" Visible="<%# config.ShowIndividualFeedLinks %>"
                                        ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + RssImageFile %>' NavigateUrl='<%# Eval("RssUrl")%>'>
                                    </portal:NoFollowHyperlink>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate></ul></FooterTemplate>
                            </asp:Repeater>
                        </asp:Panel>

                        <asp:Panel ID="divFeedEntries" runat="server" CssClass="rsscenter-rightnav rssentries">
                            <asp:UpdatePanel ID="updEntries" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" EnableViewState="true" >
                                <ContentTemplate>
                                    <asp:Literal ID="lblFeedHeading" runat="server" Visible="false" EnableViewState="false" />
                                    <asp:Repeater ID="rptEntries" runat="server" EnableViewState="true">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandName="Confirm" CommandArgument='<%#DataBinder.Eval(Container, "DataItem.EntryHash") + "_" + Convert.ToString(DataBinder.Eval(Container, "DataItem.Confirmed")) %>'
                                                ID="ConfirmBtn" runat="server" ImageUrl='<%# ConfirmImage + (DataBinder.Eval(Container, "DataItem.Confirmed").ToString() == "true" ? "done_cover.png" : "plus.png")%>'
                                                Visible='<%# EnableInPlaceEditing %>' AlternateText='<%# Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.Confirmed"))?Resources.FeedResources.EntryPublishTrueAlternateText:Resources.FeedResources.EntryPublishFalseAlternateText %>' />
                                            <div class='<%#"rssfeedentry" + DataBinder.Eval(Container, "DataItem.Confirmed") %>' id="divFeedEntry" runat="server" enableviewstate="false">
                                                <div class="rsstitle">
                                                    <%# FormatTitle(Eval("Link").ToString(), Eval("Title").ToString()) %>
                                                </div>
                                                <div class="rssdate" id="divDate" runat="server" visible='<%# config.ShowDate %>' enableviewstate="false">
                                                    <%# GetDateHeader((DateTime)DataBinder.Eval(Container, "DataItem.PubDate"))%>
                                                </div>
                                                <div class="rssfeedname" id="div2" runat="server" visible='<%# config.ShowFeedNameBeforeContent %>' enableviewstate="false">
                                                    <asp:HyperLink ID="Hyperlink6" runat="server" Visible='<%# !displaySettings.UseNoFollowOnFeedSiteLinks %>' EnableViewState="false" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.BlogUrl")%>'>
												    <%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.FeedName").ToString())%>
                                                    </asp:HyperLink>
                                                    <portal:NoFollowHyperlink ID="Hyperlink8" runat="server" Visible='<%# displaySettings.UseNoFollowOnFeedSiteLinks %>' EnableViewState="false" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.BlogUrl")%>'>
												    <%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.FeedName").ToString())%>
                                                    </portal:NoFollowHyperlink>
                                                </div>
                                                <div class="rsstext" id="divFeedBody" runat="server" enableviewstate="false" visible='<%# config.ShowItemDetail && !displaySettings.ForceShowHeadingsOnly %>'>
                                                    <NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" TrustedImageUrlPattern='<%# allowedImageUrlRegexPattern %>'
                                                        ClientScriptUrl="~/ClientScript/NeatHtml.js" Visible='<%# useNeatHtml %>' EnableViewState="false">
                                                        <%# FormatBody(Eval("Description").ToString(), Eval("Link").ToString())%>
                                                    </NeatHtml:UntrustedContent>
                                                    <div id="unfilteredContent" runat="server" enableviewstate="false" visible='<%# (!useNeatHtml) %>'>
                                                        <%# FormatBody(Eval("Description").ToString(), Eval("Link").ToString())%>
                                                    </div>
                                                </div>
                                                <div class="rssauthor" id="divAuthor" runat="server" enableviewstate="false" visible='<%# config.ShowAuthor %>'>
                                                    <asp:HyperLink ID="Hyperlink1" runat="server" Visible='<%# !displaySettings.UseNoFollowOnFeedSiteLinks %>' EnableViewState="false" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.BlogUrl")%>'>
												    <%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.Author").ToString())%>
                                                    </asp:HyperLink>
                                                    <portal:NoFollowHyperlink ID="Hyperlink9" runat="server" Visible='<%# displaySettings.UseNoFollowOnFeedSiteLinks %>' EnableViewState="false" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.BlogUrl")%>'>
												    <%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.Author").ToString())%>
                                                    </portal:NoFollowHyperlink>
                                                </div>
                                                <div class="rssfeedname" id="div1" runat="server" enableviewstate="false" visible='<%# config.ShowFeedName %>'>
                                                    <asp:HyperLink ID="Hyperlink5" runat="server" Visible='<%# !displaySettings.UseNoFollowOnFeedSiteLinks %>' EnableViewState="false" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.BlogUrl")%>'>
												    <%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.FeedName").ToString())%>
                                                    </asp:HyperLink>
                                                    <portal:NoFollowHyperlink ID="Hyperlink10" runat="server" Visible='<%# displaySettings.UseNoFollowOnFeedSiteLinks %>' EnableViewState="false" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.BlogUrl")%>'>
												    <%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.FeedName").ToString())%>
                                                    </portal:NoFollowHyperlink>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <portal:mojoCutePager ID="pgrRptEntries" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:Panel ID="divNavBottom" runat="server" CssClass="rssnavright" SkinID="plain">
                            <asp:Label ID="lblFeedListNameBottom" Font-Bold="True" runat="server"></asp:Label>
                            <a id="lnkAggregateRSSBottom" href="~/FeedManager/FeedAggregate.aspx" runat="server" class="feedlink feedag" enableviewstate="false">
                                <img alt="RSS" id="imgAggregateRSSBottom" runat="server" enableviewstate="false" /></a>
                            <portal:mojoDataList ID="dlFeedListBottom" runat="server" EnableViewState="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="editLink" runat="server" CssClass="editlink" EnableViewState="false" Text="<%# Resources.FeedResources.EditImageAltText%>"
                                        ToolTip="<%# Resources.FeedResources.EditImageAltText%>" ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
                                        NavigateUrl='<%# this.SiteRoot + "/FeedManager/FeedEdit.aspx?pageid=" + PageId.ToString() + "&amp;ItemID=" + Eval("ItemID") + "&amp;mid=" + ModuleId.ToString()  %>'
                                        Visible="<%# IsEditable %>" />
                                    <asp:HyperLink ID="Hyperlink2" runat="server" CssClass='<%# "feedlink lnk" + Eval("ItemID")%>' EnableViewState="false" Visible="<%# LinkToAuthorSite &&(!displaySettings.UseNoFollowOnFeedSiteLinks) %>" 
                                        NavigateUrl='<%# Eval("Url")%>'><%# DataBinder.Eval(Container, "DataItem.Author")%>
                                    </asp:HyperLink>
                                    <portal:NoFollowHyperlink ID="Hyperlink11" runat="server" CssClass='<%# "feedlink lnk" + Eval("ItemID")%>' EnableViewState="false" Visible="<%# LinkToAuthorSite && displaySettings.UseNoFollowOnFeedSiteLinks %>" 
                                        NavigateUrl='<%# Eval("Url")%>'><%# DataBinder.Eval(Container, "DataItem.Author")%>
                                    </portal:NoFollowHyperlink>
                                    <asp:Button ID="Button1" runat="server" Visible="<%# config.UseFeedListAsFilter %>"
                                        CommandName="filter" CommandArgument='<%# Eval("ItemID")%>' Text='<%# Eval("Author")%>' CssClass="buttonlink" />
                                    <portal:NoFollowHyperlink ID="Hyperlink3" runat="server" CssClass='<%# "feed" + Eval("ItemID")%>' EnableViewState="false" Visible="<%# config.ShowIndividualFeedLinks %>"
                                        ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + RssImageFile %>' NavigateUrl='<%# Eval("RssUrl")%>'>
                                    </portal:NoFollowHyperlink>&nbsp;&nbsp;
                                </ItemTemplate>
                            </portal:mojoDataList>
                            <asp:Repeater ID="rptFeedListBottom" runat="server" >
                                <HeaderTemplate><ul class="simplelist feedlist"></HeaderTemplate>

                                <ItemTemplate>
                                    <li>
                                        <asp:HyperLink ID="lnkEdit" runat="server" CssClass="editlink" EnableViewState="false" Text="<%# Resources.FeedResources.EditImageAltText%>"
                                        ToolTip="<%# Resources.FeedResources.EditImageAltText%>" ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
                                        NavigateUrl='<%# this.SiteRoot + "/FeedManager/FeedEdit.aspx?pageid=" + PageId.ToString() + "&amp;ItemID=" + Eval("ItemID") + "&amp;mid=" + ModuleId.ToString()  %>'
                                        Visible="<%# IsEditable %>" />
                                    <asp:HyperLink ID="lnkItem" runat="server" CssClass='<%# "feedsitelink lnk" + Eval("ItemID")%>' EnableViewState="false" Visible="<%# LinkToAuthorSite && (!displaySettings.UseNoFollowOnFeedSiteLinks) %>" 
                                        NavigateUrl='<%# Eval("Url")%>'><%# DataBinder.Eval(Container, "DataItem.Author")%>
                                    </asp:HyperLink>
                                    <portal:NoFollowHyperlink ID="HyperLink12" runat="server" CssClass='<%# "feedsitelink lnk" + Eval("ItemID")%>' EnableViewState="false" Visible="<%# LinkToAuthorSite && displaySettings.UseNoFollowOnFeedSiteLinks %>" 
                                        NavigateUrl='<%# Eval("Url")%>'><%# DataBinder.Eval(Container, "DataItem.Author")%>
                                    </portal:NoFollowHyperlink>
                                    <asp:Button ID="btnFilter" runat="server" Visible="<%# config.UseFeedListAsFilter %>"
                                        CommandName="filter" CommandArgument='<%# Eval("ItemID")%>' Text='<%# Eval("Author")%>' CssClass="buttonlink" />
                                    <portal:NoFollowHyperlink ID="lnkFeed" runat="server" CssClass='<%# "feedlink feed" + Eval("ItemID")%>' EnableViewState="false" Visible="<%# config.ShowIndividualFeedLinks %>"
                                        ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + RssImageFile %>' NavigateUrl='<%# Eval("RssUrl")%>'>
                                    </portal:NoFollowHyperlink>

                                    </li>

                                </ItemTemplate>


                                <FooterTemplate></ul></FooterTemplate>

                            </asp:Repeater>


                        </asp:Panel>


                            <mp:DataCalendar ID="dataCal1" runat="server" Visible="false"
                                EnableTheming='true'
                                SkinID="rsscal"
                                UseAccessibleHeader="true"
                                SelectionMode="Day"
                                DayField="PubDate"
                                CssClass="mpcalendarmain"
                                DayHeaderStyle-CssClass="mpcalendardayheader"
                                DayStyle-CssClass="mpcalendarday"
                                NextPrevStyle-CssClass="mpcalendarnextprevious"
                                OtherMonthDayStyle-CssClass="mpcalendarothermonth"
                                SelectedDayStyle-CssClass="mpcalendarselectedday"
                                SelectorStyle-CssClass="mpcalendarselector"
                                TitleStyle-BackColor="transparent"
                                TitleStyle-CssClass="mpcalendartitle"
                                TodayDayStyle-CssClass="mpcalendartoday"
                                WeekendDayStyle-CssClass="mpcalendarweekendday"
                                NextPrevStyle-BorderStyle="None"
                                NextPrevStyle-BorderWidth="0px"
                                DayHeaderStyle-BorderStyle="None"
                                DayHeaderStyle-BorderWidth="0px"
                                ShowGridLines="true">
                                <itemtemplate>
		                           <div class="eventcontainer">
			                            <asp:HyperLink ID="lnkItemUrl" runat="server" EnableViewState="false" NavigateUrl='<%# Container.DataItem["Link"] %>' Text='<%# Container.DataItem["Title"] %>' />
				                        </div>								
	                            </itemtemplate>
                                <noeventstemplate>
		                            <% if (config.UseFillerOnEmptyDays)
                                 {%><br /><br /><br /><% }%>
	                            </noeventstemplate>
                            </mp:DataCalendar>
                        </asp:Panel>
                    </portal:InnerBodyPanel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared" EnableViewState="false"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
</portal:OuterWrapperPanel>

