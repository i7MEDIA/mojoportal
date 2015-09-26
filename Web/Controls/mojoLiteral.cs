using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{

    public class mojoLiteral : Literal
    {
        private string excludePagesCsv = string.Empty;

        public string ExcludePagesCsv
        {
            get { return excludePagesCsv; }
            set { excludePagesCsv = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            bool show = true;

            if (excludePagesCsv.Length > 0)
            {
                string[] excludedPages = excludePagesCsv.Split(',');
                foreach (string exclude in excludedPages)
                {
                    if (Page.Request.RawUrl.Contains(exclude)) { show = false; }

                }
            }

            this.Visible = show;

        }
    }
}
