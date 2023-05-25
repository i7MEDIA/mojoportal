<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" CodeBehind="ModuleDefinitionSettings.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ModuleDefinitionSettingsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
    <asp:HyperLink ID="lnkModuleAdmin" runat="server" NavigateUrl="~/Admin/ModuleAdmin.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
    <asp:HyperLink ID="lnkModuleDefinition" runat="server"  />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin moduledefinitionsettings">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
   <mp:mojoGridView ID="grdSettings" runat="server"
     AllowPaging="false"
     AllowSorting="false"
     AutoGenerateColumns="false"
     EnableViewState="true"
     DataKeyNames="DefSettingID"
     GridLines="None"
     ShowHeader="false">
     <Columns>
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                <asp:ImageButton ID="btnEdit" runat="server" 
                    CommandName="Edit"
                    AlternateText='<%# GetEditImageAltText()%>'
                    ToolTip='<%# GetEditImageAltText()%>'
                    ImageUrl='<%# GetEditImageUrl()%>' />
            </ItemTemplate>
            <EditItemTemplate>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="">
            <ItemTemplate>
                <%# Eval("SettingName") %>
            </ItemTemplate>
            <EditItemTemplate>
                <div class="settingrow">
                    <mp:SiteLabel id="SiteLabel9" runat="server" ForControl="txtResourceFile" ConfigKey="ModuleDefinitionsResourceFileLabel" CssClass="settinglabel"/>
                    <asp:TextBox ID="txtResourceFile" Text='<%# Bind("ResourceFile")%>' runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
                 </div>
                 <div class="settingrow">
                    <mp:SiteLabel id="SiteLabel15" runat="server" ForControl="txtGroupNameKey" ConfigKey="ModuleDefinitionsGroupNameLabel" CssClass="settinglabel"/>
                    <asp:TextBox ID="txtGroupNameKey" Text='<%# Bind("GroupName")%>' runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
                 </div>
                 <div class="settingrow">
                    <mp:SiteLabel id="lbl1" runat="server" ForControl="txtSettingName" ConfigKey="ModuleDefinitionsSettingNameLabel" CssClass="settinglabel"/>
                    <asp:TextBox ID="txtSettingName" Text='<%# Bind("SettingName")%>' runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
                 </div>
                 <div class="settingrow">
                    <mp:SiteLabel id="SiteLabel2" runat="server" ForControl="ddControlType" ConfigKey="ModuleDefinitionsSettingControlTypeLabel" CssClass="settinglabel"/>
                    <asp:DropDownList ID="ddControlType" runat="server" SelectedValue='<%# Bind("ControlType") %>' CssClass="forminput">
						<asp:ListItem Value="TextBox" Text="TextBox" />
						<asp:ListItem Value="Number" Text="Number" />
						<asp:ListItem Value="Color" Text="Color" />
						<asp:ListItem Value="Password" Text="Password" />
						<asp:ListItem Value="Range" Text="Range" />
						<asp:ListItem Value="Email" Text="Email" />
                        <asp:ListItem Value="CheckBox" Text="CheckBox" />
						<asp:ListItem Value="ISettingControl" Text="ISettingControl" />
						<asp:ListItem Value="CustomField" Text="CustomField" />
						<asp:ListItem Value="" Text="Hidden" />
                    </asp:DropDownList>
                </div>
                <div class="settingrow">
                    <mp:SiteLabel id="SiteLabel3" runat="server" ForControl="txtControlSrc" ConfigKey="ModuleDefinitionsSettingControlSrcLabel" CssClass="settinglabel"/>
                    <asp:TextBox ID="txtControlSrc" runat="server" Text='<%# Bind("ControlSrc")%>' MaxLength="255" Columns="60" CssClass="forminput" />
               </div>
                <div class="settingrow">
                    <mp:SiteLabel id="SiteLabel33" runat="server" ForControl="txtSettingValue" ConfigKey="ModuleDefinitionsSettingValueLabel" CssClass="settinglabel"/>
                    <asp:TextBox ID="txtSettingValue" runat="server" Text='<%# Bind("SettingValue")%>' MaxLength="255" Columns="60" CssClass="forminput" />
               </div>
               <div class="settingrow">
                    <mp:SiteLabel id="SiteLabel10" runat="server" ForControl="txtSortOrder" ConfigKey="ModuleDefinitionsSettingSortOrderLabel" CssClass="settinglabel"/>
                    <asp:TextBox ID="txtSortOrder" runat="server" Text='<%# Bind("SortOrder")%>' MaxLength="255" Columns="60" CssClass="forminput" />
               </div>
               <div class="settingrow">
                    <mp:SiteLabel id="SiteLabel12" runat="server" ForControl="txtHelpKey" ConfigKey="ModuleDefinitionsSettingHelpKeyLabel" CssClass="settinglabel"/>
                    <asp:TextBox ID="txtHelpKey" runat="server" Text='<%# Bind("HelpKey")%>' MaxLength="255" Columns="60" CssClass="forminput" />
               </div>
               <div class="settingrow">
                    <mp:SiteLabel id="SiteLabel4" runat="server" ConfigKey="ModuleDefinitionsSettingRegexExpressionLabel" CssClass="settinglabel"/>
                    <asp:TextBox ID="txtRegexValidationExpression" runat="server"
                        Text='<%# Bind("RegexValidationExpression")%>' 
                        Rows="4" Columns="60" TextMode="MultiLine" />
               </div>
				<div class="settingrow">
					<mp:SiteLabel ID="SiteLabel16" runat="server" ConfigKey="ModuleDefinitionsSettingAttributesLabel" CssClass="settinglabel"/>
					<asp:TextBox ID="txtAttributes" runat="server"
						Text='<%# Bind("Attributes")%>'
						Rows="4" Columns="60" TextMode="MultiLine" />
				</div>

				<div class="settingrow">
					<mp:SiteLabel ID="SiteLabel17" runat="server" ConfigKey="ModuleDefinitionsSettingOptionsLabel" CssClass="settinglabel"/>
					<asp:TextBox ID="txtOptions" runat="server"
						Text='<%# Bind("Options")%>'
						Rows="4" Columns="60" TextMode="MultiLine" />
				</div>
                <div class="settingrow">
                    <mp:SiteLabel id="SiteLabel38" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                   <asp:Button id="btnGridUpdate" runat="server" Text='<%# GetUpdateButtonText() %>' CommandName="Update" />
                    <asp:Button id="btnGridCancel" runat="server" Text='<%# GetCancelButtonText() %>' CommandName="Cancel" />
                    <asp:Button id="btnGridDelete" runat="server" Text='<%# GetDeleteButtonText() %>' CommandName="Delete" />
                 </div>
            </EditItemTemplate>
        </asp:TemplateField>
     </Columns>
     <EmptyDataTemplate>
            <p class="nodata"><asp:Literal id="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
    </EmptyDataTemplate>
</mp:mojoGridView>

    <portal:HeadingControl ID="subHeading" runat="server" />
    <div class="settingrow">
        <mp:SiteLabel id="SiteLabel9" runat="server" ForControl="txtNewResourceFile" ConfigKey="ModuleDefinitionsResourceFileLabel" CssClass="settinglabel"/>
        <asp:TextBox ID="txtNewResourceFile" Text='<%# Bind("ResourceFile")%>' runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
    </div>
    <div class="settingrow">
        <mp:SiteLabel id="SiteLabel14" runat="server" ForControl="txtGroupNameKey" ConfigKey="ModuleDefinitionsGroupNameLabel" CssClass="settinglabel"/>
        <asp:TextBox ID="txtGroupNameKey" runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
   </div>
    <div class="settingrow">
        <mp:SiteLabel id="SiteLabel5" runat="server" ForControl="txtNewSettingName" ConfigKey="ModuleDefinitionsSettingNameLabel" CssClass="settinglabel"/>
        <asp:TextBox ID="txtNewSettingName" runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
   </div>
    <div class="settingrow">
        <mp:SiteLabel id="SiteLabel6" runat="server" ForControl="ddNewControlType" ConfigKey="ModuleDefinitionsSettingControlTypeLabel" CssClass="settinglabel"/>
        <asp:DropDownList ID="ddNewControlType" runat="server" CssClass="forminput">
            <asp:ListItem Value="TextBox" Text="TextBox" />
            <asp:ListItem Value="CheckBox" Text="CheckBox" />
            <asp:ListItem Value="ISettingControl" Text="ISettingControl" />
			<asp:ListItem Value="CustomField" Text="CustomField" />
			<asp:ListItem Value="" Text="Hidden" />
		</asp:DropDownList>
    </div>
    <div class="settingrow">
        <mp:SiteLabel id="SiteLabel7" runat="server" ForControl="txtNewControlSrc" ConfigKey="ModuleDefinitionsSettingControlSrcLabel" CssClass="settinglabel"/>
        <asp:TextBox ID="txtNewControlSrc" runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
    </div>
     <div class="settingrow">
        <mp:SiteLabel id="SiteLabel77" runat="server" ForControl="txtNewSettingValue" ConfigKey="ModuleDefinitionsSettingValueLabel" CssClass="settinglabel"/>
        <asp:TextBox ID="txtNewSettingValue" runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
    </div>
    <div class="settingrow">
        <mp:SiteLabel id="SiteLabel11" runat="server" ForControl="txtNewSortOrder" ConfigKey="ModuleDefinitionsSettingSortOrderLabel" CssClass="settinglabel"/>
        <asp:TextBox ID="txtNewSortOrder" runat="server" Text="500" MaxLength="255" Columns="60" CssClass="forminput" />
    </div>
    <div class="settingrow">
        <mp:SiteLabel id="SiteLabel13" runat="server" ForControl="txtNewHelpKey" ConfigKey="ModuleDefinitionsSettingHelpKeyLabel" CssClass="settinglabel"/>
        <asp:TextBox ID="txtNewHelpKey" runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
    </div>
     <div class="settingrow">
            <mp:SiteLabel id="SiteLabel8" runat="server" ForControl="txtNewRegexValidationExpression" ConfigKey="ModuleDefinitionsSettingRegexExpressionLabel" CssClass="settinglabel"/>
            <asp:TextBox ID="txtNewRegexValidationExpression" runat="server" Rows="4" Columns="60"
                TextMode="MultiLine" CssClass="forminput" />
     </div>
	<div class="settingrow">
		<mp:SiteLabel ID="SiteLabel19" runat="server" ForControl="txtAttributes" ConfigKey="ModuleDefinitionsSettingAttributesLabel" CssClass="settinglabel"/>
		<asp:TextBox ID="txtAttributes" runat="server" Rows="4" Columns="60"
			TextMode="MultiLine" CssClass="forminput" />
	</div>
	<div class="settingrow">
		<mp:SiteLabel ID="SiteLabel18" runat="server" ConfigKey="ModuleDefinitionsSettingOptionsLabel" CssClass="settinglabel" />
		<asp:TextBox ID="txtOptions" runat="server"
			Text='<%# Bind("Options")%>'
			Rows="4" Columns="60" TextMode="MultiLine" />
	</div>
      <div class="settingrow">
        <mp:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
            <portal:mojoButton ID="btnCreateNewSetting" runat="server" Text='' />
            <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="moduledefinitionsettingshelp" />
       </div>

</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
