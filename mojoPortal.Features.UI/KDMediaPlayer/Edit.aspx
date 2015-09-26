<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Edit.aspx.cs" Inherits="mojoPortal.MediaPlayerUI.EditMediaPlayerPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper mediaedit">
            <portal:HeadingControl ID="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                    <fieldset>
                        <legend>
                            <mp:SiteLabel ID="AddTrackFieldsetLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="AddTrackFieldsetLabel"/>
                        </legend>
                        <asp:Panel ID="AddTrackPanel" runat="server">
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
                            <div class="settingrow">
                                <mp:SiteLabel ID="SelectFileSiteLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="SelectFileSiteLabelText" CssClass="settinglabel" />
                                <asp:TextBox ID="MediaFileTextBox" runat="server" CssClass="forminput widetextbox" />
                                <portal:FileBrowserTextBoxExtender ID="MediaFileBrowser" runat="server" BrowserType="media" />&nbsp;
                                <asp:Button ID="AddMediaFileLinkButton" runat="server" ValidationGroup="FileValidation" CssClass="buttonlink" />
                                <portal:mojoHelpLink ID="FileLocationHelpLink" runat="server" HelpKey="MediaPlayerFileLocation-help" />
                                <asp:RequiredFieldValidator ID="MediaFileRequiredValidator" runat="server" ControlToValidate="MediaFileTextBox" ValidationGroup="FileValidation" />
                                <asp:RegularExpressionValidator ID="MediaFileRegularExpValidator" runat="server" ControlToValidate="MediaFileTextBox" ValidationGroup="FileValidation" />
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="SelectedFilesSiteLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="SelectedFilesSiteLabelText" CssClass="settinglabel" />
                                <asp:ListBox ID="SelectedFilesListBox" runat="server" Width="450" />
                                <portal:mojoHelpLink ID="SelectedFilesHelpLink" runat="server" HelpKey="MediaPlayerSelectedFiles-help" />
                                <asp:CustomValidator ID="AtLeastOneFileSelectedValidator" runat="server" ControlToValidate="SelectedFilesListBox" ValidationGroup="TrackValidation" ValidateEmptyText="true" />
                            </div>
                            <portal:mojoButton ID="AddTrackButton" runat="server" ValidationGroup="TrackValidation" />
                            <portal:mojoButton ID="CancelButton" runat="server" CausesValidation="false" />
                            <br /><br />
                            <asp:Label ID="AddTrackErrorLabel" runat="server" CssClass="txterror" />
                            <asp:ValidationSummary ID="AddFileValidationSummary" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="FileValidation" />
                            <asp:ValidationSummary ID="AddTrackValidationSummary" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="TrackValidation" />
                        </asp:Panel>
                    </fieldset>
                    <br />
                    <fieldset>
                        <legend>
                            <mp:SiteLabel ID="ManageTracksFieldSetLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="ManageTracksFieldSetLabel" />
                        </legend>
                        <asp:Panel ID="ManageTracksPanel" runat="server">
                            <mp:mojoGridView ID="TracksGridView" runat="server" DataKeyNames="TrackID" DataSourceID="TracksObjectDataSource" AutoGenerateColumns="false">
                                <EmptyDataTemplate>
                                    <mp:SiteLabel ID="NoTracksSiteLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="NoTracksSiteLabelText" CssClass="txterror" />
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <mp:SiteLabel ID="TrackOrderHeaderLabel" runat="server" ResourceFile="MediaPlayerResources"
                                                ConfigKey="TrackOrderHeaderLabelText" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="width:150px;">
                                            <portal:mojoButton ID="UpButton" runat="server" CausesValidation="false" CommandName="MoveUp" CommandArgument='<%# Eval("TrackOrder") %>' />
                                            <portal:mojoButton ID="DownButton" runat="server" CausesValidation="false" CommandName="MoveDown" CommandArgument='<%# Eval("TrackOrder") %>' />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <mp:SiteLabel ID="TrackNameHeaderLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="TrackNameHeaderLabelText" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# Eval("Name") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <mp:SiteLabel ID="TrackArtistHeaderLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="TrackArtistHeaderLabelText" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# Eval("Artist") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <mp:SiteLabel ID="TrackFilesHeaderLabel" runat="server" ResourceFile="MediaPlayerResources" ConfigKey="TrackFilesHeaderLabelText" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:ListView ID="FileListview" runat="server">
                                                <LayoutTemplate>
                                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("FilePath") %>
                                                </ItemTemplate>
                                                <ItemSeparatorTemplate>
                                                    <br />
                                                </ItemSeparatorTemplate>
                                            </asp:ListView>
                                            <asp:ObjectDataSource ID="FilesObjectDataSource" runat="server" SelectMethod="GetForTrack"
                                                TypeName="mojoPortal.Features.Business.MediaFile">
                                                <SelectParameters>
                                                    <asp:Parameter Name="trackID" Type="Int32" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="EditTrackLinkButton" runat="server" CausesValidation="false" CommandName="EditTrack" CommandArgument='<%# Eval("TrackID") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </mp:mojoGridView>
                            <asp:ObjectDataSource ID="TracksObjectDataSource" runat="server" SelectMethod="GetForPlayer" 
                                TypeName="mojoPortal.Features.Business.MediaTrack">
                            </asp:ObjectDataSource>
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
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
