<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="EditTrack.aspx.cs" Inherits="mojoPortal.MediaPlayerUI.EditTrackPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper mediaedit">
            <portal:HeadingControl ID="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                    <fieldset>
                        <legend>
                            <mp:SiteLabel ID="MediaTrackPropertiesFieldsetLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="MediaTrackPropertiesFieldsetLabel" />
                        </legend>
                        <asp:Panel ID="MediaTrackPropertiesPanel" runat="server">
                            <div class="settingrow">
                                <mp:SiteLabel ID="MediaTypeLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="PlayerTypeLabelText" CssClass="settinglabel" />
                                <asp:Label ID="MediaTypeValueLabel" runat="server" />
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="TrackNameLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="EditAudioPlayerTrackNameLabel" CssClass="settinglabel" />
                                <asp:TextBox ID="TrackNameTextBox" runat="server" MaxLength="100" CssClass="forminput widetextbox" />
                                <portal:mojoHelpLink ID="TrackNameHelpLink" runat="server" HelpKey="MediaPlayerTrackName-help" />
                                <asp:RequiredFieldValidator ID="TrackNameRequiredValidator" runat="server" ControlToValidate="TrackNameTextBox" ValidationGroup="TrackValidation" />
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="ArtistLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="EditAudioPlayerArtistLabel" CssClass="settinglabel" />
                                <asp:TextBox ID="ArtistTextBox" runat="server" MaxLength="100" CssClass="forminput widetextbox" />
                                <portal:mojoHelpLink ID="ArtistHelpLink" runat="server" HelpKey="MediaPlayerArtist-help" />
                                <asp:RequiredFieldValidator ID="ArtistRequiredValidator" runat="server" ControlToValidate="ArtistTextBox" ValidationGroup="TrackValidation" />
                            </div>
                            <br />
                            <portal:mojoButton ID="UpdateTrackButton" runat="server" ValidationGroup="TrackValidation" />
                            <portal:mojoButton ID="DeleteTrackButton" runat="server" CausesValidation="false" />
                            <portal:mojoButton ID="CancelButton" runat="server" CausesValidation="false" />
                            <asp:ValidationSummary ID="UpdateTrackValidationSummary" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="TrackValidation" />
                        </asp:Panel>
                    </fieldset>
                    <br />
                    <fieldset>
                        <legend>
                            <mp:SiteLabel ID="ManageMediaFilesFieldsetLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="ManageMediaFilesFieldsetLabel" />
                        </legend>
                        <asp:Panel ID="ManageMediaFilesPanel" runat="server">
                            <mp:mojoGridView ID="MediaFilesGridView" runat="server" DataKeyNames="FileID" DataSourceID="FilesObjectDataSource" AutoGenerateColumns="false">
                                <EmptyDataTemplate>
                                    <mp:SiteLabel ID="NoFilesSiteLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="NoFilesSiteLabel" CssClass="txterror" />
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <mp:SiteLabel ID="FileHeaderTextSiteLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="FileHeaderTextSiteLabel" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# Eval("FilePath") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="DeleteFileLinkButton" runat="server" CausesValidation="false" CommandName="DeleteFile" CommandArgument='<%# Eval("FileID") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </mp:mojoGridView>
                            <asp:ObjectDataSource ID="FilesObjectDataSource" runat="server" SelectMethod="GetForTrack" TypeName="mojoPortal.Features.Business.MediaFile">
                            </asp:ObjectDataSource>
                            <br />
                            <div class="settingrow">
                                <mp:SiteLabel ID="SelectFileSiteLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="SelectFileSiteLabelText" CssClass="settinglabel" />
                                <asp:TextBox ID="MediaFileTextBox" runat="server" Width="350" />
                                <portal:FileBrowserTextBoxExtender ID="MediaFileBrowser" runat="server" BrowserType="media" />&nbsp;
                                <asp:Button ID="AddMediaFileLinkButton" runat="server" ValidationGroup="FileValidation" CssClass="buttonlink" />
                                <portal:mojoHelpLink ID="FileLocationHelpLink" runat="server" HelpKey="MediaPlayerFileLocation-help" />
                                <asp:RequiredFieldValidator ID="MediaFileRequiredValidator" runat="server" ControlToValidate="MediaFileTextBox" ValidationGroup="FileValidation" />
                                <asp:RegularExpressionValidator ID="MediaFileRegularExpValidator" runat="server" ControlToValidate="MediaFileTextBox" ValidationGroup="FileValidation" />
                            </div>
                            <asp:Label ID="AddFileErrorLabel" runat="server" CssClass="txterror" />
                            <asp:ValidationSummary ID="AddFileValidationSummary" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="FileValidation" />
                        </asp:Panel>
                    </fieldset>
                </portal:InnerBodyPanel>
            </portal:OuterBodyPanel>
            <portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared">
            </portal:EmptyPanel>
        </portal:InnerWrapperPanel>
        <mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
