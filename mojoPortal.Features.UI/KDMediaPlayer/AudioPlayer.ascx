<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AudioPlayer.ascx.cs" Inherits="mojoPortal.MediaPlayerUI.AudioPlayer" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper AudioPlayer">
        <portal:ModuleTitleControl runat="server" ID="TitleControl" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
            <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                <mp:SiteLabel ID="SetupNeededLabel" runat="server" ConfigKey="SetupNeededLabel" ResourceFile="MediaPlayerResources" Visible="false" />
                <asp:Literal ID="litUpperContent" runat="server" />
                <div id="PlayerInstance" runat="server" class="jp-jplayer"></div>
                <div id="PlayerContainer" runat="server" class="jp-audio">
                    <div class="jp-type-playlist">
                        <div class="jp-gui jp-interface">
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
                            <div class="jp-progress">
                                <div class="jp-seek-bar">
                                    <div class="jp-play-bar"></div>
                                </div>
                            </div>
                            <div class="jp-volume-bar">
                                <div class="jp-volume-bar-value"></div>
                            </div>
                            <div class="jp-time-holder">
                                <div class="jp-current-time"></div>
                                <div class="jp-duration"></div>
                            </div>
                            <ul class="jp-toggles">
                                <li id="ShuffleControl" runat="server"><a id="ShuffleLink" runat="server" href="javascript:;" class="jp-shuffle" tabindex="1"></a></li>
                                <li id="ShuffleOffControl" runat="server"><a id="ShuffleOffLink" runat="server" href="javascript:;" class="jp-shuffle-off" tabindex="1"></a></li>
                                <li><a id="RepeatLink" runat="server" href="javascript:;" class="jp-repeat" tabindex="1"></a></li>
                                <li><a id="RepeatOffLink" runat="server" href="javascript:;" class="jp-repeat-off" tabindex="1"></a></li>
                            </ul>
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
            </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared">
        </portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
