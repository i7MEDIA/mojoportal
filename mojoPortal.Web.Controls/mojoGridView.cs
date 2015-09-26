using System;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls
{
    // http://www.asp.net/cssadapters/gridview.aspx
    // http://www.mojoportal.com/Forums/Thread.aspx?thread=2276&mid=34&pageid=5&ItemID=4

    /// <summary>
    /// The only purposeof this control is because we use the CSS Control Adapter to extend it
    /// rather thasn extend the standard GridView directly. This allows the standard GridView to work
    /// as expected for those who don't want to use the Adapter.
    /// 
    /// </summary>
    public class mojoGridView : GridView
    {
        private string tableCssClass = string.Empty;

        public string TableCssClass
        {
            get { return tableCssClass; }
            set { tableCssClass = value; }
        }

        private bool renderCellSpacing = true; //true for backward compat

        public bool RenderCellSpacing
        {
            get { return renderCellSpacing; }
            set { renderCellSpacing = value; }
        }

        private bool renderTableId = true; //true for backward compat

        public bool RenderTableId
        {
            get { return renderTableId; }
            set { renderTableId = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //if (Page.IsPostBack) { return; }
            //if (this.Rows.Count == 0) { this.Visible = false; }
        }

        
    }
}
