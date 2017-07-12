<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="BlogNav.ascx.cs" Inherits="mojoPortal.Web.BlogUI.BlogNav" %>
<%@ Register TagPrefix="blog" TagName="TagList" Src="~/Blog/Controls/CategoryListControl.ascx" %>
<%@ Register TagPrefix="blog" TagName="Archives" Src="~/Blog/Controls/ArchiveListControl.ascx" %>
<%@ Register TagPrefix="blog" TagName="FeedLinks" Src="~/Blog/Controls/FeedLinksControl.ascx" %>
<%@ Register TagPrefix="blog" TagName="StatsControl" Src="~/Blog/Controls/StatsControl.ascx" %>
<%@ Register TagPrefix="blog" TagName="RelatedPostsList" Src="~/Blog/Controls/RelatedPosts.ascx" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>
<%@ Register TagPrefix="blog" TagName="SearchBox" Src="~/Blog/Controls/SearchBox.ascx" %>

<blog:BlogDisplaySettings ID="displaySettings" runat="server" />

<blog:BlogNavPanel ID="divNav" runat="server">
	<blog:SearchBox ID="searchBox" runat="server" />

	<asp:Calendar runat="server"
		CaptionAlign="Top"
		CssClass="aspcalendarmain"
		DayHeaderStyle-CssClass="aspcalendardayheader"
		DayNameFormat="FirstLetter"
		DayStyle-CssClass="aspcalendarday"
		FirstDayOfWeek="sunday"
		ID="calBlogNav"
		NextMonthText="+"
		NextPrevFormat="CustomText"
		NextPrevStyle-CssClass="aspcalendarnextprevious"
		OtherMonthDayStyle-CssClass="aspcalendarothermonth"
		PrevMonthText="-"
		SelectedDayStyle-CssClass="aspcalendarselectedday"
		SelectorStyle-CssClass="aspcalendarselector"
		ShowDayHeader="true"
		ShowGridLines="false"
		ShowNextPrevMonth="true"
		ShowTitle="true"
		SkinID="Blog"
		TitleFormat="MonthYear"
		TitleStyle-CssClass="aspcalendartitle"
		TodayDayStyle-CssClass="aspcalendartoday"
		WeekendDayStyle-CssClass="aspcalendarweekendday"
	></asp:Calendar>

	<%-- jQuery Date Picker --%>
	<asp:Panel ID="pnlDatePicker" runat="server" EnableViewState="false" CssClass="blogcal" />

	<blog:FeedLinks ID="Feeds" runat="server" />

	<blog:BlogTopSideBarPanel ID="pnlSideTop" runat="server" CssClass="bsidecontent bsidetop">
		<asp:Literal ID="litUpperSidebar" runat="server" />
	</blog:BlogTopSideBarPanel>

	<blog:BlogStatsPanel ID="pnlStatistics" runat="server" CssClass="bsidelist bstatslist">
		<blog:StatsControl ID="stats" runat="server" />
	</blog:BlogStatsPanel>

	<blog:BlogCatListPanel ID="pnlCategories" runat="server" CssClass="bsidelist bcatlist">
		<blog:TagList ID="tags" runat="server" />
	</blog:BlogCatListPanel>

	<blog:RelatedPostsList ID="relatedPosts" runat="server" />

	<blog:BlogArchiveListPanel ID="pnlArchives" runat="server" CssClass="bsidelist barchivelist">
		<blog:Archives ID="archive" runat="server" />
	</blog:BlogArchiveListPanel>

	<blog:BlogBottomSideBarPanel ID="pnlSideBottom" runat="server" CssClass="bsidecontent bsidebottom">
		<asp:Literal ID="litLowerSidebar" runat="server" />
	</blog:BlogBottomSideBarPanel>
</blog:BlogNavPanel>
