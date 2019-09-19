<%@ Page language="c#" Codebehind="EventDetails.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.EventCalendarUI.EventCalendarViewEvent" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop id="ctop1" runat="server" />
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper eventcalendar">
            <portal:HeadingControl ID="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
		    <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
		        <mp:SiteLabel id="Sitelabel1" runat="server" ConfigKey="EventCalendarEditStartTimeLabel" ResourceFile="EventCalResources"></mp:SiteLabel>
				<asp:Label id="lblStartTime" runat="server"></asp:Label>
				<br />
				<mp:SiteLabel id="Sitelabel2" runat="server" ConfigKey="EventCalendarEditEndTimeLabel" ResourceFile="EventCalResources"></mp:SiteLabel>
				<asp:Label id="lblEndTime" runat="server"></asp:Label>
				<mp:SiteLabel runat="server" ConfigKey="LocationLabel" ResourceFile="EventCalResources" />
				<asp:Label ID="lblLocation" runat="server" />
				<br /><br />
				<div>
				<asp:Literal id="litDescription" runat="server" />
				</div>
				<portal:LocationMap ID="gmap" runat="server" CssClass="gmap_event"></portal:LocationMap>
		    </portal:InnerBodyPanel>
            </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
		</portal:InnerWrapperPanel>
	<mp:CornerRounderBottom id="cbottom1" runat="server" />	
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

