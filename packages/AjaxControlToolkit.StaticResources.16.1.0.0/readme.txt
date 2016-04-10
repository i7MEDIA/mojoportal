The AjaxControlToolkit.StaticResources package contains files (scripts, styles, and images) for bundling (Microsoft Web Optimization).
 
This package adds:
1) ~/Content/AjaxControlToolkit and ~/Scripts/AjaxControlToolkit folders;
2) Script and style bundles;
3) The AjaxControlToolkit section in the web.config file.
 
Additional changes to be made manually:
 
1) Add a ScriptReference to the ScriptManager control
 
    <asp:ScriptManager runat="server">
        <Scripts>
            ...
            <asp:ScriptReference Path="~/Scripts/AjaxControlToolkit/Bundle" />
        </Scripts>
    </asp:ScriptManager>
 
2) Add the Styles.Render expression to the <head> element.
 
    <asp:PlaceHolder runat="server">
        <%: System.Web.Optimization.Styles.Render("~/Content/AjaxControlToolkit/Styles/Bundle") %>
    </asp:PlaceHolder>

Note that bundling and minification is enabled or disabled by setting the value of the debug attribute in the compilation element in the Web.config file.
In Debug mode this value is set to true, so bundling and minification is disabled:

    <system.web>
        <compilation debug="true" />
        <!-- Lines removed for clarity. -->
    </system.web>

To enable bundling and minification, set the debug value to "false". You can override the Web.config setting with the EnableOptimizations property on the BundleTable class. 