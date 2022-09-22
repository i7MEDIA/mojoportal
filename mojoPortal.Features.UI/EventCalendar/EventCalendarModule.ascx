<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EventCalendarModule.ascx.cs" Inherits="mojoPortal.Web.EventCalendarUI.EventCalendar" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper eventcalendar eventcalendarbasic" >
    <portal:ModuleTitleControl id="Title1" runat="server"  />
    <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
    <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <mp:DataCalendar id="cal1"  runat="server"  
            EnableTheming='true' 
            SkinID="eventcal"
            UseAccessibleHeader="true"
            SelectionMode="Day"
            DayField="EventDate"
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
            ShowGridLines="true"
            >
	    <ItemTemplate>
		    <div class="eventcontainer">
			    <asp:HyperLink Text="<%# Resources.EventCalResources.EventCalendarEditEventLink%>" Tooltip="<%# Resources.EventCalResources.EventCalendarEditEventLink%>" 
			    id="editLink" 
			    ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
			    NavigateUrl='<%# this.SiteRoot + "/EventCalendar/EditEvent.aspx?pageid=" + PageId.ToString() + "&ItemID=" + Container.DataItem["ItemID"] + "&mid=" + ModuleId.ToString()  %>' 
			    Visible="<%# IsEditable %>" runat="server" />
			    <a class="eventlink" href='<%# SiteRoot + "/EventCalendar/EventDetails.aspx?ItemID=" + Container.DataItem["ItemID"] + "&mid=" + Container.DataItem["ModuleID"] + "&pageid=" + PageId %>'>
			    <% if (config.ShowTimeInMonthView) { %><strong><%# DateTime.Parse(Container.DataItem["StartTime"].ToString()).ToString("t") %></strong>: <%} %>
				<%# Server.HtmlEncode(Container.DataItem["Title"].ToString()) %></a>
			</div>
	    </ItemTemplate>
	    <NoEventsTemplate>
		    <% if(config.UseFillerOnEmptyDays) {%><br /><br /><br /><% }%>
	    </NoEventsTemplate>
    </mp:DataCalendar>
    </portal:InnerBodyPanel>
    </portal:OuterBodyPanel>
    <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
