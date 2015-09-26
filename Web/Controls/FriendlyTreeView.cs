using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// this control just inherits from TreeView and uses the standard CSS Friendly adapter
    /// http://cssfriendly.codeplex.com/
    /// http://www.asp.net/cssadapters/TreeView.aspx
    /// whereas mojoTreeView uses a highly modified version
    /// usage <portal:FriendlyTreeView id="yourTree" runat="server" />
    /// </summary>
    public class FriendlyTreeView : TreeView
    {
    }
}