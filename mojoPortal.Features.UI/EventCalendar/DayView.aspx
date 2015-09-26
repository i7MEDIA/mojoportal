<%@ Page language="c#" Codebehind="DayView.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.EventCalendarUI.EventCalendarDayView" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop id="ctop1" runat="server" />
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper eventcalendar">
            <portal:HeadingControl ID="heading" runat="server" />
		    <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
		    <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
		    <asp:repeater id="rptEvents" runat="server" EnableViewState="false">
			    <itemtemplate>
			        <h3>
			            <%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.Title").ToString())%>
			            <%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.StartTime")).ToShortTimeString()%> - <%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.EndTime")).ToShortTimeString()%>
			        </h3>
                     <%# DataBinder.Eval(Container, "DataItem.Description").ToString()%>
			    </itemtemplate>
		    </asp:repeater>
		    </portal:InnerBodyPanel>
		    </portal:OuterBodyPanel>
		    <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
		</portal:InnerWrapperPanel>
		<mp:CornerRounderBottom id="cbottom1" runat="server" />	
		</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
