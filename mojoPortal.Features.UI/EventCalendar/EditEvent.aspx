<%@ Page language="c#" Codebehind="EditEvent.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.EventCalendarUI.EventCalendarEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper eventcalendaredit">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">

<asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate" >
	<div class="settingrow">
	    <mp:SiteLabel id="lblTitle" runat="server" ForControl="txtTitle" CssClass="settinglabel" ConfigKey="EventCalendarEditTitleLabel" ResourceFile="EventCalResources" ></mp:SiteLabel>
	    <asp:TextBox id="txtTitle" runat="server"  Columns="50" maxlength="100" CssClass="forminput widetextbox"></asp:TextBox>
	</div>
	<div class="settingrow">
		<mp:SiteLabel ID="Sitelabel3" runat="server" ForControl="fckDescription" ConfigKey="EventCalendarEditDescriptionLabel" ResourceFile="EventCalResources" CssClass="settinglabel" />
	</div>
	<div class="settingrow">
		<mpe:EditorControl ID="edContent" runat="server"></mpe:EditorControl>
	</div>
	<div class="settingrow">
	    <mp:SiteLabel id="lblStartDate" runat="server" ForControl="dpEventDate" CssClass="settinglabel" ConfigKey="EventCalendarEditEventDateLabel" ResourceFile="EventCalResources" ></mp:SiteLabel>			
	    <mp:DatePickerControl id="dpEventDate" runat="server" ShowTime="False" CssClass="forminput" SkinID="eventcalendar"></mp:DatePickerControl>
	</div>
	<div class="settingrow">
	    <mp:SiteLabel id="Sitelabel4" runat="server" ForControl="ddStartTime" CssClass="settinglabel" ConfigKey="EventCalendarEditStartTimeLabel" ResourceFile="EventCalResources" ></mp:SiteLabel>
	    <asp:DropDownList ID="ddStartTime" runat="server" EnableTheming="false" CssClass="forminput"></asp:DropDownList>
	</div>
	<div class="settingrow">
	    <mp:SiteLabel id="Sitelabel2" ForControl="ddEndTime" runat="server" CssClass="settinglabel" ConfigKey="EventCalendarEditEndTimeLabel" ResourceFile="EventCalResources" ></mp:SiteLabel>
	    <asp:DropDownList ID="ddEndTime" runat="server" EnableTheming="false" CssClass="forminput"></asp:DropDownList>
	</div>
	<div class="settingrow locationrow">
		<mp:SiteLabel id="SiteLabel1" runat="server" ForControl="txtLocation" CssClass="settinglabel" ConfigKey="LocationLabel" ResourceFile="EventCalResources" ></mp:SiteLabel>
		<asp:TextBox id="txtLocation" runat="server"  Columns="50" maxlength="100" CssClass="forminput widetextbox"></asp:TextBox>
		<mp:SiteLabel ID="lblShowMap" runat="server" ForControl="showMap" ConfigKey="ShowMap" ResourceFile="EventCalResources" UseLabelTag="true" >
			<asp:CheckBox ID="chkShowMap" runat="server" />
		</mp:SiteLabel>
	</div>
	<div class="settingrow">
<mp:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
    <div class="forminput">
	    <portal:mojoButton id="btnUpdate" runat="server" Text="Update" />&nbsp;
		<portal:mojoButton  id="btnDelete" runat="server" Text="Delete this item" CausesValidation="false" />&nbsp;
		<asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;	
		<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="eventedithelp" />
		</div>
	</div>
<asp:HiddenField ID="hdnReturnUrl" runat="server" />
</asp:Panel>	

</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
<portal:SessionKeepAliveControl id="ka1" runat="server" />	
		
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
