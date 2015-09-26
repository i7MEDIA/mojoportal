<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="Monitor.aspx.cs" Inherits="mojoPortal.Web.AdminUI.MonitorPage" %>

<asp:content contentplaceholderid="leftContent" id="MPLeftPane" runat="server" />
<asp:content contentplaceholderid="mainContent" id="MPContent" runat="server">
    <portal:mojoPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
        <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
        <asp:panel id="pnl1" runat="server" cssclass="panelwrapper ">
            <portal:mojoPanel ID="MojoPanel1" runat="server" ArtisteerCssClass="art-PostContent">
                <div class="modulecontent">
                    <h2>
                        Memory and CPU Consumption</h2>
                    <p class="container">
                        <b>Total Bytes Allocated:</b>
                        <asp:literal id="litTotalAllocatedMemorySize" runat="server" />
                        Bytes<br />
                        <br />
                        <b>Total Bytes In Use:</b>
                        <asp:literal id="litSurvivedMemorySize" runat="server" />
                        Bytes<br />
                        <br />
                        <b>CPU usage:</b>
                        <asp:literal id="litTotalProcessorTime" runat="server" />
                        Milliseconds
                    </p>
                    <asp:repeater id="rpt" runat="server">
                        <headertemplate>
                            <h2>Exceptions</h2>
                        </headertemplate>
                        <itemtemplate>
                            <br /><a href="javascript:" class="ExpLink"><%# Eval("Url")%></a><br /><br />
                            <div  class="outerExpContent">
                            <asp:Repeater id="rptExceptions" runat="server">
                                <ItemTemplate>
                                    <div class="ExpContent">
                                        <p>
                                            <b>Exception Type:</b> <%# ((Exception)Container.DataItem).GetType().FullName %></p>
                                        <p>
                                            <b>Message:</b> <%# Eval("Message")%> </p>
                                        <p>
                                            <b>Stack Trace:</b> <%# Eval("StackTrace")%> </p>
                                            <hr />    
                                    </div>    
                                </ItemTemplate>
                                </asp:Repeater>

                            </div>

                            </itemtemplate>
                        
                    </asp:repeater>
                </div>
            </portal:mojoPanel>
            <div class="cleared">
            </div>
        </asp:panel>
        <mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
    </portal:mojoPanel>
</asp:content>
<asp:content contentplaceholderid="rightContent" id="MPRightPane" runat="server" />
<asp:content contentplaceholderid="pageEditContent" id="MPPageEdit" runat="server" />
