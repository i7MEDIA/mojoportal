<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="VideoPlayer.ascx.cs"
    Inherits="mojoPortal.MediaPlayerUI.VideoPlayer" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper AudioPlayer">
        <portal:ModuleTitleControl runat="server" ID="TitleControl" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
            <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                <mp:SiteLabel ID="SetupNeededLabel" runat="server" ConfigKey="SetupNeededLabel" ResourceFile="MediaPlayerResources" Visible="false" />
                <asp:Literal ID="litUpperContent" runat="server" />
                <asp:Panel ID="PlayerPanel" runat="server">
                    <div id="PlayerContainer" runat="server" class="jp-video jp-video-270p">
                        <div class="jp-type-playlist">
                            <div id="PlayerInstance" runat="server" class="jp-jplayer"></div>
                            <div class="jp-gui">
                                <div class="jp-video-play">
                                    <a id="VideoPlayLink" runat="server" href="javascript:;" class="jp-video-play-icon" tabindex="1"></a>
                                </div>
                                <div class="jp-interface">
                                    <div class="jp-progress">
                                        <div class="jp-seek-bar">
                                            <div class="jp-play-bar"></div>
                                        </div>
                                    </div>
                                    <div class="jp-current-time"></div>
                                    <div class="jp-duration"></div>
                                    <div class="jp-controls-holder">
                                        <ul class="jp-controls">
                                            <li><a id="PreviousLink" runat="server" href="javascript:;" class="jp-previous" tabindex="1"></a></li>
                                            <li><a id="PlayLink" runat="server" href="javascript:;" class="jp-play" tabindex="1"></a></li>
                                            <li><a id="PauseLink" runat="server" href="javascript:;" class="jp-pause" tabindex="1"></a></li>
                                            <li><a id="NextLink" runat="server" href="javascript:;" class="jp-next" tabindex="1"></a></li>
                                            <li><a id="StopLink" runat="server" href="javascript:;" class="jp-stop" tabindex="1"></a></li>
                                            <li><a id="MuteLink" runat="server" href="javascript:;" class="jp-mute" tabindex="1"></a></li>
                                            <li><a id="UnmuteLink" runat="server" href="javascript:;" class="jp-unmute" tabindex="1"></a></li>
                                            <li><a id="MaxVolumeLink" runat="server" href="javascript:;" class="jp-volume-max" tabindex="1"></a></li>
                                        </ul>
                                        <div class="jp-volume-bar">
                                            <div class="jp-volume-bar-value"></div>
                                        </div>
                                        <ul class="jp-toggles">
                                            <li id="FullScreenControl" runat="server"><a id="FullScreenLink" runat="server" href="javascript:;" class="jp-full-screen" tabindex="1"></a></li>
                                            <li id="RestoreScreenControl" runat="server"><a id="RestoreScreenLink" runat="server" href="javascript:;" class="jp-restore-screen" tabindex="1"></a></li>
                                            <li id="ShuffleControl" runat="server"><a id="ShuffleLink" runat="server" href="javascript:;" class="jp-shuffle" tabindex="1"></a></li>
                                            <li id="ShuffleOffControl" runat="server"><a id="ShuffleOffLink" runat="server" href="javascript:;" class="jp-shuffle-off" tabindex="1"></a></li>
                                            <li><a id="RepeatLink" runat="server" href="javascript:;" class="jp-repeat" tabindex="1"></a></li>
                                            <li><a id="RepeatOffLink" runat="server" href="javascript:;" class="jp-repeat-off" tabindex="1"></a></li>
                                        </ul>
                                    </div>
                                    <div class="jp-title">
                                        <ul>
                                            <li></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="jp-playlist">
                                <ul>
                                    <li></li>
                                </ul>
                            </div>
                            <div class="jp-no-solution">
                                <asp:Literal ID="NoSolutionLiteral" runat="server" />
                            </div>
                        </div>
                    </div>
                    <asp:Literal ID="litLowerContent" runat="server" />
                </asp:Panel>
            </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared">
        </portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
